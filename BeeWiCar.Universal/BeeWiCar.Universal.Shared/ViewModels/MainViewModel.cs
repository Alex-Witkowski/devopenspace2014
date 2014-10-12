namespace BeeWiCar.Universal.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private string hello;

        public MainViewModel()
        {
            this.Hello = "Hallo Leipzig";
        }

        public string Hello
        {
            get
            {
                return this.hello;
            }

            set
            {
                this.SetProperty(ref this.hello, value);
            }
        }
    }
}
