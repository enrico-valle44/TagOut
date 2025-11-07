using Microsoft.EntityFrameworkCore;
using PrototipoService.DTO;
using PrototipoService.Entities;
using PrototipoService.Model;
using PrototipoService.Services.Interface;


namespace PrototipoService.Services;

public class UserInfoService : IUserInfoService
{
    private readonly DatabaseContext _context;
    //readony perche non vogliamo che l'oggetto venga modificato in nessun metodo della classe (per sbaglip magari) 
    //costruttore ctor tab
    public UserInfoService(DatabaseContext context) //MI SERVE UNA VARIABILE DATABASECONTEXT 
    {
        _context = context;
    }

    public async Task<int> AddUserInfo(UserInfoDTO AddEntity)
    {
        //per prima cosa controlliamo se esista uno user con questo nome

        //query da fare sul db con le lambda 
        //la facciamo async la funzione, quindi va variabile exist sara await
        bool exist = await _context.UserInfo.AsQueryable().Where(x=>x.Username == AddEntity.Username).AnyAsync();
        //se esiste bad request

        if (exist == true)
        {
            return -1;
        }
         
        var entityToAdd = new UserInfo
        {
            Username = AddEntity.Username,
            Gender = AddEntity.Gender,
            DOB = AddEntity.DOB,
        };

        await _context.UserInfo.AddAsync(entityToAdd);
        await _context.SaveChangesAsync(); //cosi viene salvata la risorsa 

        return entityToAdd.Id; //torna l'id dell'entita aggiunta 
    }

    public async Task<int> Delete(int id)
    {
        UserInfo ? entity = await _context.UserInfo.FindAsync(id); //recuperiamo la risorsa per id
        //se esiste bad request

        if (entity == null) 
        {
            return -1; //se non esiste bad request
        }

        _context.Remove(entity); //rimane in memoria ma non viene fatto sul db

        await _context.SaveChangesAsync(); //cosi lo fa sul db

        return entity.Id;
    }

    public async Task<UserInfoViewModel> GetByUsername(string username)
    {
        UserInfo? entity = await _context.UserInfo.FirstOrDefaultAsync(p => p.Username.Equals(username));

        UserInfoViewModel entityViewModel = null;
        if (entity != null)
        {
            entityViewModel = new UserInfoViewModel
            {
                Id = entity.Id,
                Username = entity.Username,
                Gender = entity.Gender,
                DOB = entity.DOB,

            };
        }

        return entityViewModel;
    }

    public async Task<UserInfoViewModel> GetById(int id)
    {
        UserInfo? entity = await _context.UserInfo.FindAsync(id);

        UserInfoViewModel entityViewModel = null;
        if (entity != null)
        {
            entityViewModel = new UserInfoViewModel
            {
                Id = entity.Id,
                Username = entity.Username,
                Gender = entity.Gender,
                DOB = entity.DOB,

            };
        }

        return entityViewModel;
    }

    public async Task<List<UserInfoViewModel>> GetByFilter(UserInfoFilter filter)
    {
        IQueryable<UserInfo> query = _context.UserInfo.AsQueryable(); //serve per costruire le query sul db???

        if(!string.IsNullOrWhiteSpace(filter.Username)) //controllo piu preciso per questo
        {
            query = query.Where(x=> x.Username == filter.Username);
        }

        if (filter.Gender != null)
        {
            query = query.Where(x => x.Gender == filter.Gender);
        }

        if (filter.DOB != null)
        {
            query = query.Where(x => x.DOB == filter.DOB);
        }
        //dal db ci restituisce una lista di userInfo, ma brutta
        List<UserInfo> list = await query.ToListAsync(); //facciamo richiesta a risorsa esterna
        //la costruimao con viewmodel nostro modello fatto apposta
        List<UserInfoViewModel> listViewModel = list.Select(x => new UserInfoViewModel
        {
            Id = x.Id,
            Username = x.Username,
            Gender = x.Gender,
            DOB = x.DOB,

        }).ToList();

        return listViewModel;

    }

    public async Task<int> UpdatePatch(UserInfoUpdateDTO updateEntity, int id)
    {
        UserInfo? entity = await _context.UserInfo.FindAsync(id);

        if (entity == null)
        {
            return -1;
        }

        if (!string.IsNullOrWhiteSpace(updateEntity.Username))
        {
           entity.Username = updateEntity.Username;
        }

        if (!string.IsNullOrWhiteSpace(updateEntity.Gender))
        {
            entity.Gender = updateEntity.Gender;
        }

        if (updateEntity.DOB != null)
        {
            entity.DOB = updateEntity.DOB.Value; //.value specifica che dentro c'è un valore, il che è una certezza per via dell'if
        }



        await _context.SaveChangesAsync();


        return entity.Id;
    }
}
