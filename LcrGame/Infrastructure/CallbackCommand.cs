using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LcrGame.Infrastructure
{
    /// <summary>
    /// Example:  <Button  Command="{Binding CallbackCommand}" CommandParameter="OnShowModules" >
    /// </summary>
    public class CallbackCommand : ICommand
    {
        object _viewModel;

        public CallbackCommand(object viewModel)
        {
            _viewModel = viewModel;
        }

#pragma warning disable CS0067 // Member hides inherited member; missing new keyword
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067 // Member hides inherited member; missing new keyword

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object methodName)
        {
            try
            {
                string MethodName = methodName.ToString();
                object methodContext = _viewModel;

                if (MethodName.Contains("."))
                {
                    var methodComponents = MethodName.Split('.');

                    for (int i = 0; i < methodComponents.Count() - 1; i++)
                    {
                        methodContext = methodContext.GetType().GetProperty(methodComponents[i]).GetValue(methodContext);
                    }

                    methodContext.GetType().InvokeMember(methodComponents.Last(), System.Reflection.BindingFlags.InvokeMethod, null, methodContext, null);
                }
                else
                {
                    methodContext.GetType().InvokeMember(MethodName, System.Reflection.BindingFlags.InvokeMethod, null, methodContext, null);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception CallbackCommand Invoke command: " + e.Message);
            }
        }
    }
}
