import { NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Router } from '@angular/router';
import { Utente } from '../../model/utente';
import { AccessService } from '../../services/access-service/access-service';

@Component({
  selector: 'app-register',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatSelectModule,
    NgIf
  ],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {

  private formBuilder = inject(FormBuilder);
  private accessoServ = inject(AccessService);
  private router = inject(Router);

  oggi = new Date().toISOString().split('T')[0];

  accessoForm = this.formBuilder.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
    dataNascita: ['', [Validators.required]],
    gender: ['', [Validators.required]],
  });


  salva(): void {
    this.accessoServ.salvaInfoUtente(this.accessoForm.value as Utente);
    this.accessoServ.postUtente(this.accessoForm.value as Utente);
    //this.accessoServ.salvaId();
    this.router.navigateByUrl('/map');
  }
}
