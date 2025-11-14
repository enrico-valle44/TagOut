import { Component, inject } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { DataService } from './../../services/data-service/data-service';
import { LocationService } from '../../services/location-service/location-service';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { LocalStorageService } from '../../services/local-storage-service/local-storage.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-new-report',
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIcon,
    MatSelectModule,
  ],
  templateUrl: './new-report.html',
  styleUrl: './new-report.scss',
})
export class NewReport {
  private fb = new FormBuilder();
  public dataServ = inject(DataService);
  private locationServ = inject(LocationService);
  private localStorageServ = inject(LocalStorageService);

  public categoryNames: string[] = [];
  public images: string[] = [];
  //public selectedFiles: File[] = [];

  public reportForm = this.fb.group({
    title: ['', Validators.required],
    description: [''],
    categories: this.fb.array([this.fb.control('')]),
  });

  constructor(private router: Router) {
    this.dataServ.getCategories().then((categoryNames: string[]) => {
      this.categoryNames = categoryNames;
    });
  }

  get categories() {
    return this.reportForm.get('categories') as FormArray;
  }

  addCategoryInput() {
    this.categories.push(this.fb.control(''));
  }

  removeCategoryInput(index: number) {
    console.log(index);
    this.categories.removeAt(index);
  }

  //crea base64
  onImageSelected(event: Event) {
    const element = event.target as HTMLInputElement;
    if (element.files && element.files.length > 0) {
      const file = element.files[0];
      const reader = new FileReader();
      reader.onload = () => {
        this.images.push(reader.result as string);
      };
      reader.readAsDataURL(file);
    }
  }

  async postReport() {
    const newReport = {
      title: this.reportForm.value.title!,
      description: this.reportForm.value.description!,
      categories: this.reportForm.value.categories as string[],
      images: this.images,
      dateReport: new Date().toISOString(),
      lat: 0,
      lng: 0,
    };

    const pos = await this.locationServ.getPosition();
    newReport.lat = pos.coords.latitude;
    newReport.lng = pos.coords.longitude;

    await this.dataServ.postReport(newReport, this.localStorageServ.getId());

    //alert('Il report è stato pubblicato !');

    this.reportForm.reset();
    this.images = [];
    this.categories.clear();

    this.router.navigateByUrl('/map');
  }

  removeImage(index: number) {
    this.images.splice(index, 1);
  }

  goBack() {
    this.router.navigate(['/map']);
  }

  hasSelectedCategory(): boolean {
    const categories = this.reportForm.get('categories')?.value as (
      | string
      | null
    )[];
    return categories?.some((cat) => cat != null && cat.trim() !== '');
  }

  /*onImageSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (!input.files) return;

    for (let i = 0; i < input.files.length; i++) {
      const file = input.files[i];
      this.selectedFiles.push(file);
    }
  }

  async postReport() {
    const newReport = {
      title: this.reportForm.value.title!,
      description: this.reportForm.value.description!,
      categories: this.reportForm.value.categories as string[],
      dateReport: new Date().toISOString(),
      images: [],
      lat: 0,
      lng: 0,
    };

    const pos = await this.locationServ.getPosition();
    newReport.lat = pos.coords.latitude;
    newReport.lng = pos.coords.longitude;

    await this.dataServ.postReport(newReport, this.localStorageServ.getId());
    const reportId = await this.dataServ.postReport(
      newReport,
      this.localStorageServ.getId()
    );

    for (const file of this.selectedFiles) {
      const formData = new FormData();
      formData.append('file', file);
      formData.append('reportId', reportId);

      const imgResponse = await fetch(
        `http://localhost:5089/Image/upload/report/${reportId}`,
        {
          method: 'POST',
          body: formData,
        }
      );

      if (!imgResponse.ok) console.error('Errore upload immagine', file.name);
    }

    alert('Report e immagini caricati con successo!');

    // reset form
    this.reportForm.reset();
    this.images = [];
    this.selectedFiles = [];
    this.categories.clear();

    this.router.navigateByUrl('/map');
  }*/
}
