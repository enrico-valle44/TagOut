import { Component, inject } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { DataService } from './../../services/data-service/data-service';

@Component({
  selector: 'app-new-report',
  imports: [ReactiveFormsModule],
  templateUrl: './new-report.html',
  styleUrl: './new-report.scss',
})
export class NewReport {
  private fb = new FormBuilder();
  public dataServ = inject(DataService);
  public categoryNames: string[] = [];

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

  postReport() {
    console.log(this.reportForm.valid);
    console.log(this.reportForm.value);
  }
}
