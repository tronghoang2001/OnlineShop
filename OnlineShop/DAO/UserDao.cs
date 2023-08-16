using OnlineShop.Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using OnlineShop.Commons;

namespace OnlineShop.DAO
{
    public class UserDao
    {
        OnlineShopDbContext db = null;
        public UserDao() 
        { 
            db = new OnlineShopDbContext();
        }
        public User GetById (string username)
        {
            return db.Users.SingleOrDefault(x => x.Username == username);
        }

        public User ViewDetail(int id)
        {
            return db.Users.Find(id);
        }
        public int Login(string username, string password)
        {
            var rs = db.Users.SingleOrDefault(x => x.Username == username); 
            if(rs == null)
            {
                return 0;
            }
            else
            {
                if(rs.Status == false)
                {
                    return -1;
                }
                else
                {
                    if (rs.Password == password)
                    {
                        return 1;
                    }
                    else
                        return -2;
                }
            }
        }
        public List<string> GetListCredential(string username)
        {
            var user = db.Users.Single(x => x.Username == username);
            var data = (from a in db.Credentials
                       join b in db.UserGroups on a.UserGroupID equals b.ID
                       join c in db.Roles on a.RoleID equals c.ID
                       where b.ID == user.GroupID
                       select new
                       {
                           RoleID = a.RoleID,
                           UserGroupID = a.UserGroupID
                       }).AsEnumerable().Select(x=>new Credential()
                       {
                           RoleID = x.RoleID,
                           UserGroupID = x.UserGroupID
                       });
            return data.Select(x=>x.RoleID).ToList();
        }

        public int Login(string username, string password, bool isLoginAdmin = false)
        {
            var rs = db.Users.SingleOrDefault(x => x.Username == username);
            if (rs == null)
            {
                return 0;
            }
            else
            {
                if(isLoginAdmin == true)
                {
                    if(rs.GroupID == CommonConstants.ADMIN_GROUP || rs.GroupID == CommonConstants.MOD_GROUP)
                    {
                        if (rs.Status == false)
                        {
                            return -1;
                        }
                        else
                        {
                            if (rs.Password == password)
                            {
                                return 1;
                            }
                            else
                                return -2;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    if (rs.Status == false)
                    {
                        return -1;
                    }
                    else
                    {
                        if (rs.Password == password)
                        {
                            return 1;
                        }
                        else
                            return -2;
                    }
                }
            }
        }

        public IEnumerable<User> ListAllPaging(string searchString, int page, int pageSize) 
        {
            IQueryable<User> model = db.Users;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Username.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);
        }

        public long Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public long InsertForFacebook(User entity)
        {
            var user = db.Users.SingleOrDefault(x => x.Username == entity.Username);
            if (user == null)
            {
                db.Users.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            else
            {
                return user.ID;
            }

        }

        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.ID);
                user.Name = entity.Name;
                if(!string.IsNullOrEmpty(entity.Password) && entity.Password != EncryptorMD5.MD5Hash(user.Password)) 
                {
                    user.Password = entity.Password;
                }
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.Phone = entity.Phone;
                user.ModifiedBy = entity.ModifiedBy;
                user.Status = entity.Status;
                user.ModifiedDate = DateTime.Now;
           
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                //logging
                return false;
            }
        }

        public bool Delete(int id) 
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }

        public bool CheckUserName (string userName)
        {
            return db.Users.Count(x=>x.Username == userName) > 0;   
        }
        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }
    }
}