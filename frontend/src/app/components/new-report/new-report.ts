import { Component, inject } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { DataService } from './../../services/data-service/data-service';
import { Report } from '../../model/report';
import { LocationService } from '../../services/location-service/location-service';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';

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
  public categoryNames: string[] = [];
  public images: string[] = [];
  private locationServ = inject(LocationService);

  constructor() {
    this.dataServ.getCategories().then((categories) => {
      this.categoryNames = categories;
    });
  }

  public reportForm = this.fb.group({
    title: ['', Validators.required],
    description: [''],
    categories: this.fb.array([this.fb.control('')]),
  });

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

  postReport() {
    const newReport = {
      title: this.reportForm.value.title!,
      description: this.reportForm.value.description!,
      categories: this.reportForm.value.categories as string[],
      images: this.images,
      date: new Date().toISOString(),
      lat: 0,
      lng: 0,
    };
    this.locationServ.getPosition().then((pos) => {
      newReport.lat = pos.coords.latitude;
      newReport.lng = pos.coords.longitude;
    });
    this.dataServ.postReport(newReport);
  }
}
