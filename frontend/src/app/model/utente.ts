export interface Utente {
    id?: number;
    userName: string;
    dataNascita: string;
    gender: 'M' | 'F' | 'A';
}