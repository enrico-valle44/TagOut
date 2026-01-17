# TagOut🖌️
**TagOut** è un'applicazione web per la gestione dei report su graffiti e vandalismi.  
Gli utenti possono creare **report** contenenti:

- Titolo e descrizione  
- Posizione geografica (rilevata automaticamente o scelta sulla mappa)  
- Immagini  
- Categoria di appartenenza dei graffiti (**Sport**, **Politica**, **Cultura**)

Il progetto utilizza le seguenti tecnologie:

- **Backend:** .NET 8 con Entity Framework Core  
- **Frontend:** Angular con TypeScript  
- **Database:** PostgreSQL  
- Tutti i componenti sono containerizzati e orchestrati tramite **Docker Compose**, rendendo semplice l'avvio dell'intero stack.  

⚠️ L'app è ancora in fase di sviluppo: alcune funzionalità, come la gestione degli utenti, sono basate su **LocalStorage** e non su account con password.
## 📌 Prerequisiti

- **Docker Desktop** 
- **Git** (per clonare il repository).  
- **Node.js** (solo se vuoi eseguire il frontend localmente senza Docker)  
- **.NET SDK 8.0** (solo se vuoi eseguire il backend localmente senza Docker)  
- **IDE o editor di testo** a tua scelta, ad esempio Visual Studio Code

- File `.env`

Il progetto utilizza un file `.env` per definire le credenziali e configurazioni del database. Anche se si usa solo Docker, **serve creare il file `.env` nella cartella principale del progetto** con il seguente contenuto minimo:

```env
DB_HOST=postgres-db
DB_PORT=5432
DB_NAME=prova
DB_USER=utente
DB_PASSWORD=password123
```

## 🏁Avvio del progetto con Docker

Il progetto è strutturato con **Docker Compose**, quindi backend, frontend e database PostgreSQL verranno eseguiti in container separati.

### 1. Clonare il repository

```bash
git clone https://github.com/enrico-valle44/TagOut.git
cd TagOut
```
### 2. Build e avvio dei container

Assicurati di trovarti nella cartella principale del progetto (dove c’è il `docker-compose.yml` e il file `.env`).

Costruisci e avvia tutti i container eseguendo il comando:

```bash
docker compose up --build
```
### 3. Accesso al frontend e testing delle API

Una volta avviati i container, puoi accedere al frontend dell'applicazione dal browser:

- **Frontend Angular/NGINX**: [http://localhost:8080](http://localhost:8080)  
  Qui potrai visualizzare l’interfaccia dell’app e provare a creare report, caricare immagini, ecc.

- **Backend .NET API**: [http://localhost:5089](http://localhost:5089)  
  Le API sono raggiungibili tramite questo URL. Ad esempio, puoi testare endpoint come:
  - `POST /Report/add/{userId}` – per creare un nuovo report
  - `GET /Report/all` – per ottenere tutti i report
  - `POST /Image/upload/report/{reportId}` – per caricare immagini associate a un report

💡 Per esplorare tutti gli endpoint e testare le chiamate direttamente, visita lo **[Swagger UI](http://localhost:5089/swagger)**
> ℹ️ Tutte le richieste al backend saranno gestite dal database PostgreSQL in esecuzione all’interno del container. Non serve avere un database locale.

## 🖼️ Risorse Grafiche

All'interno del progetto è presente la cartella `Graffiti`, che contiene alcune immagini di esempio relative alle categorie di graffiti utilizzate nell'applicazione.  
Queste immagini possono essere utilizzate per testare l'upload e la visualizzazione dei report nel sistema.