using PrototipoService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototipoService.Services.Interface;

public interface IUserInfoService
{
    //firme dei metodi
    Task<int> AddUserInfo(UserInfoDTO AddEntity);
    Task<UserInfoViewModel> GetById(int Id);
    Task<List<UserInfoViewModel>> GetByFilter(UserInfoFilter filter);
    Task<int> UpdatePatch(UserInfoUpdateDTO updateEntity, int id); 
    Task<int> Delete(int id);


}
