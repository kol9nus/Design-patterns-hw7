using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework7
{
    public class CopyContext
    {
        public IState state { get; set; }
        public readonly int price = 10;


        public CopyContext()
        {
            state = new PayState();
        }

        public void Pay(int amount)
        {
            state.Pay(this, amount);
        }

        public void ChooseDevice(string device)
        {
            state.ChooseDevice(this, device);
        }

        public void ChooseDoc(string name)
        {
            state.ChooseDoc(this, name);
        }

        public void PrintDoc()
        {
            state.PrintDoc(this);
        }

        public int GiveChange()
        {
            return state.GiveChange(this);
        }
    }

    public interface IState
    {
        void Pay(CopyContext copyContext, int amount);
        void ChooseDevice(CopyContext copyContext, string device);
        void ChooseDoc(CopyContext copyContext, string name);
        void PrintDoc(CopyContext copyContext);
        int GiveChange(CopyContext copyContext);
    }

    public abstract class StateBase : IState
    {
        protected int Balance { get; set; }
        public StateBase(int balance)
        {
            Balance = balance;
        }

        public abstract void Pay(CopyContext copyContext, int amount);

        public abstract void ChooseDevice(CopyContext copyContext, string device);

        public abstract void ChooseDoc(CopyContext copyContext, string name);

        public abstract void PrintDoc(CopyContext copyContext);

        public int GiveChange(CopyContext copyContext)
        {
            int i = Balance;
            Balance = 0;
            copyContext.state = new PayState();
            return i;
        }
    }

    public class PayState : StateBase
    {
        public PayState(int balance) : base(balance)
        {
        }

        public PayState() : base(0)
        {
        }

        public override void PrintDoc(CopyContext copyContext)
        {
            throw new Exception("Pay first!");
        }
        public override void ChooseDevice(CopyContext copyContext, string device)
        {
            throw new Exception("Pay first!");
        }

        public override void ChooseDoc(CopyContext copyContext, string name)
        {
            throw new Exception("Pay first!");
        }

        public override void Pay(CopyContext copyContext, int amount)
        {
            if (amount <= 0)
            {
                return;
            }
            Console.WriteLine($"Баланс: {amount}");
            copyContext.state = new ChooseDeviceState(amount);
        }
    }

    public class ChooseDeviceState : StateBase
    {

        public static readonly string DEVICE_WIFI = "WiFi";
        public static readonly string DEVICE_USB = "USB";

        public ChooseDeviceState(int balance) : base(balance)
        {
        }

        public override void ChooseDevice(CopyContext copyContext, string device)
        {
            if (device.Equals(DEVICE_USB))
            {
                copyContext.state = new ChooseDocFromUSBState(Balance);
                return;
            }
            if (device.Equals(DEVICE_WIFI))
            {
                copyContext.state = new ChooseDocFromWiFiState(Balance);
                return;
            }
            throw new Exception("Invalid device");
        }

        public override void ChooseDoc(CopyContext copyContext, string name)
        {
            throw new Exception("Choose device first!");
        }

        public override void PrintDoc(CopyContext copyContext)
        {
            throw new Exception("Choose device first!");
        }

        public override void Pay(CopyContext copyContext, int amount)
        {
            throw new Exception("You already paid!");
        }
    }

    public abstract class ChooseDocBase : StateBase
    {
        public ChooseDocBase(int balance) : base(balance)
        {
        }

        public override void ChooseDevice(CopyContext copyContext, string device)
        {
            throw new Exception("You already chose!");
        }

        public override void PrintDoc(CopyContext copyContext)
        {
            throw new Exception("Choose doc first!");
        }

        public override void Pay(CopyContext copyContext, int amount)
        {
            throw new Exception("You already paid!");
        }
    }

    public class ChooseDocFromUSBState : ChooseDocBase
    {
        public ChooseDocFromUSBState(int balance) : base(balance)
        {
        }
        public override void ChooseDoc(CopyContext copyContext, string name)
        {
            copyContext.state = new PrintDocFromUSBState(Balance, name);
        }
}

    public class ChooseDocFromWiFiState : ChooseDocBase
    {
        public ChooseDocFromWiFiState(int balance) : base(balance)
        {
        }
        public override void ChooseDoc(CopyContext copyContext, string name)
        {
            copyContext.state = new PrintDocFromWiFiState(Balance, name);
        }
    }

    public abstract class PrintDocumentStateBase : StateBase
    {
        protected string DocumentName { get; }
        protected string DeviceType { get; set; }
        public PrintDocumentStateBase(int balance, string name) : base(balance)
        {
            DocumentName = name;
        }

        public override void ChooseDevice(CopyContext copyContext, string device)
        {
            throw new Exception("You already chose!");
        }

        public override void ChooseDoc(CopyContext copyContext, string name)
        {
            throw new Exception("You already chose!");
        }

        public override void PrintDoc(CopyContext copyContext)
        {
            Console.WriteLine($"Печатаем с {DeviceType}, документ с именем {DocumentName}");
            Balance -= copyContext.price;
            if (Balance < copyContext.price)
            {
                Console.WriteLine($"Осталось денег: {Balance}, внесите ещё");
                copyContext.state = new PayState(Balance);
            }
            else
            {
                Console.WriteLine($"Осталось денег: {Balance}");
                ReturnOneStateBack(copyContext);
            }
        }

        public override void Pay(CopyContext copyContext, int amount)
        {
            throw new Exception("You already paid!");
        }

        protected abstract void ReturnOneStateBack(CopyContext copyContext);
    }

    public class PrintDocFromUSBState : PrintDocumentStateBase
    {
        public PrintDocFromUSBState(int balance, string name) : base(balance, name)
        {
            DeviceType = "USB";
        }

        protected override void ReturnOneStateBack(CopyContext copyContext)
        {
            copyContext.state = new ChooseDocFromUSBState(Balance);
        }
    }

    public class PrintDocFromWiFiState : PrintDocumentStateBase
    {
        public PrintDocFromWiFiState(int balance, string name) : base(balance, name)
        {
            DeviceType = "WiFi";
        }

        protected override void ReturnOneStateBack(CopyContext copyContext)
        {
            copyContext.state = new ChooseDocFromWiFiState(Balance);
        }
    }
}
