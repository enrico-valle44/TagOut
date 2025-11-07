import { Component, inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Accesso as AccessoService } from '../../services/access-service/accesso-service';
import { Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, MatOption } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { Utente } from '../../model/utente';
import { MatIcon } from '@angular/material/icon';
@Component({
  selector: 'app-accesso',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    MatOption, MatIcon,
  ],
  templateUrl: './accesso.html',
  styleUrl: './accesso.scss',
})
export class Accesso {
  private formBuilder = inject(FormBuilder);
  private accessoServ = inject(AccessoService);
  private router = inject(Router);

  oggi = new Date().toISOString().split('T')[0];

  accessoForm = this.formBuilder.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
    dataNascita: ['', [Validators.required]],
    gender: ['', [Validators.required]],
  });

  salva(): void {
  this.accessoServ.salvaUtente(this.accessoForm.value as Utente);
  this.router.navigateByUrl('/');
}
}
