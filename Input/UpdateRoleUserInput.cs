namespace WeatherOfCity.Input
{
    public class UpdateRoleUserInput
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool role { get; set; } = false;
    }
}
