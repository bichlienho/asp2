using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project.Repository
{
    public class AccountRepository:IAccountRepository
    {
        private readonly DatabaseContext db;
        public AccountRepository(DatabaseContext db)
        {
            this.db = db;
        }
        public async Task<Account> CheckLogin(string email, string password)
        {
            // Tìm tài khoản với email và kiểm tra nếu tài khoản còn hoạt động
            var account = await db.Accounts
                .Include(a => a.Role) // Bao gồm các AccountRoles (liên kết giữa tài khoản và vai trò)
                .FirstOrDefaultAsync(a => a.Email == email && a.IsActive == true); // Tìm tài khoản theo email và kiểm tra trạng thái Enable

            if (account != null)
            {
                // Nếu tìm thấy tài khoản, so sánh mật khẩu đã băm
                if (BCrypt.Net.BCrypt.Verify(password, account.Password))
                {
                    // Nếu mật khẩu chính xác, trả về tài khoản
                    return account;
                }
            }
            // Nếu không tìm thấy tài khoản hoặc mật khẩu không đúng, trả về null
            return null;
        }

        public async Task<Account> Register(Account account)
        {
            // Check if an account with the same email or username already exists
            var existingAccount = await db.Accounts
                .FirstOrDefaultAsync(a => a.Email == account.Email || a.Username == account.Username);

            if (existingAccount != null)
            {
                throw new Exception("An account with this email or username already exists.");
            }

            // Ensure that the PhoneNumber is not empty
            if (string.IsNullOrEmpty(account.PhoneNumber))
            {
                throw new Exception("PhoneNumber is required.");
            }

            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            // Check account status or role if applicable
            account.IsActive = true;  // Make sure the status is properly set
            account.RoleId = 2; // Assign a default role (like a regular user)

            // Save the account after validating it
            await db.Accounts.AddAsync(account);
            await db.SaveChangesAsync();  // Confirm the data is saved

            return account;  // Return the created account
        }




        public async Task<IEnumerable<Account>> GetAllAccounts()
        {
            return await db.Accounts.ToListAsync();
        }

        public async Task<Account> Update(Account account)
        { //check nó đã tồn tại chưa
            var acc = await GetAccount(account.Id);
            if (acc is null)
            {
                return null;
            }
            acc.Username = account.Username;
            acc.Fullname = account.Fullname;
            acc.Email = account.Email;
            acc.PhoneNumber = account.PhoneNumber;
            db.Accounts.Update(acc);
            await db.SaveChangesAsync();
            return account;

        }
        public async Task<Account> GetAccount(int id)
        {
            return await db.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        }
        //discount
        public async Task<Discount> Create(Discount discount)
        {
            db.Discounts.Add(discount);
            int result = await db.SaveChangesAsync();
            if (result == 1)
            {
                return discount;
            }
            return null;
        }

    
    }
}
