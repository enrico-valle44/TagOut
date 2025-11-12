export interface Utente {
    id?: number;
    username: string;
    dataNascita: string;
    gender: 'M' | 'F' | 'A';
}