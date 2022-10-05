using Dapper;
using System.Data;
using System.Data.SqlClient;
using WeatherOfCity.Input;

namespace WeatherOfCity.Sevices
{
    public class UserSevices : IUserSevices
    {
        private readonly IConfiguration configuration;

        public UserSevices(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Insert_UpdateUser(UpdateRoleUserInput input)
        {
            using(var connect =new SqlConnection(configuration.GetSection("ConnectionStrings:constring").Value))
            {
                var result = new DynamicParameters();
                result.Add("@UserName",input.UserName);
                result.Add("@PassWord", input.Password);
                result.Add("@email", input.Email);
                result.Add("@role", input.role);
                var temp = connect.Query<UserInput>(
                    "SP_UPDATE_ROLE_USER",
                    result,
                    commandType: CommandType.StoredProcedure
                    );
            }
        }
    }
}
