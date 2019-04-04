using System;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {


        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            _context = context;

        }

        #region login
        public async Task<User> Login(string username, string password)
        {
            //on stock le user saisie en paramenete dans une variable s'il match la donnée en bd
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }


            return user;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            //on instancie lobjet hash avec la Key pour avoir acces au password hashé
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                //on hash le password saisie et on le stock  computedHash
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    //on compare le password saisie hashé avec le password dans la bd hashé
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        #endregion

        #region register
        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            //method responsible for hashing
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
            
        }

            //method responsable du hashage et de la générationd de clé salt appelé dans la méthode register
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            //hmac est un objet disposable
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                // on créé une clé pour sécuriser le hash
                passwordSalt = hmac.Key;
                //on hash le password
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
#endregion
        
        
        
        public async Task<bool> UserExists(string username)
        {
            if (await _context.Users.AnyAsync(x=> x.Username == username))
            {
                return true;
            }

            return false;
        }
    }
}