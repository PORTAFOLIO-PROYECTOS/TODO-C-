using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_Introducción
{
    class _5_Try_Catch
    {
        /*
         
        La instrucción try-catch consta de un bloque try seguido de una o más cláusulas catch que especifican controladores para diferentes excepciones.

        Cuando se produce una excepción, Common Language Runtime (CLR) busca la instrucción catch que controla esta excepción. 
        Si el método que se ejecuta actualmente no contiene un bloque catch, CLR busca el método que llamó el método actual, 
        y así sucesivamente hasta la pila de llamadas. Si no existe ningún bloque catch, CLR muestra al usuario un mensaje de 
        excepción no controlada y detiene la ejecución del programa.

        El bloque try contiene el código protegido que puede producir la excepción. El bloque se ejecuta hasta que se produce 
        una excepción o hasta que se completa correctamente. Por ejemplo, el intento siguiente de convertir un objeto null produce 
        la excepción NullReferenceException:

            object o2 = null;  
            try  
            {  
                int i2 = (int)o2;   // Error  
            } 


        Aunque la cláusula catch puede utilizarse sin argumentos para detectar cualquier tipo de excepción, no se recomienda este uso. 
        En general, solo debe convertir las excepciones que sabe cómo recuperar. Por lo tanto, debe especificar siempre un argumento de 
        objeto derivado de System.Exception Por ejemplo:

            catch (InvalidCastException e)   
            {  
            }  


        Es posible utilizar más de una cláusula catch específica en la misma instrucción try-catch. En este caso, el orden de las cláusulas 
        catch es importante, puesto que las cláusulas catch se examinan por orden. Detectar las excepciones más específicas antes que las 
        menos específicas. El compilador genera un error si ordena los bloques de detección para que un bloque posterior nunca pueda alcanzarse.

        La utilización de los argumentos catch es una manera de filtrar las excepciones que desea controlar. 
        También se puede usar una expresión de filtro que examine aún más la excepción para decidir si controlarla. 
        Si la expresión de filtro devuelve false, prosigue la búsqueda de un controlador.

            catch (ArgumentException e) when (e.ParamName == "…")  
            {  
            }  


        Los filtros de excepción son preferibles para detectar y volver a producir (se explica a continuación) porque los filtros dejan la pila intacta. 
        Si un controlador posterior vuelca la pila, puede ver la procedencia original de la excepción, más que solo la ubicación en la que se volvió a producir. 
        Un uso común de las expresiones de filtro de excepciones es el registro. Puede crear una función de filtro que siempre devuelva false y que también 
        resulte en un registro, o bien puede registrar excepciones a medida que se produzcan sin tener que controlarlas y volver a generarlas.

        Se puede usar una instrucción throw en un bloque catch para volver a iniciar la excepción detectada por la instrucción catch. 
        En el ejemplo siguiente se extrae información de origen de una excepción IOException y, a continuación, se produce la excepción al método principal.

            catch (FileNotFoundException e)  
            {  
                // FileNotFoundExceptions are handled here.  
            }  
            catch (IOException e)  
            {  
                // Extract some information from this exception, and then   
                // throw it to the parent method.  
                if (e.Source != null)  
                    Console.WriteLine("IOException source: {0}", e.Source);  
                throw;  
            }  


        Puede capturar una excepción y producir una excepción diferente. Al hacerlo, especifique la excepción que detectó como excepción interna, 
        tal como se muestra en el ejemplo siguiente.

            catch (InvalidCastException e)   
            {  
                // Perform some action here, and then throw a new exception.  
                throw new YourCustomException("Put your error message here.", e);  
            }  


        También se puede volver a producir una excepción sin una condición específica es true, tal y como se muestra en el ejemplo siguiente.

            catch (InvalidCastException e)  
            {  
                if (e.Data == null)  
                {  
                    throw;  
                }  
                else  
                {  
                    // Take some action.  
                }  
            }  


        //Nota
        // También es posible usar un filtro de excepción para obtener un resultado similar de una forma generalmente más limpia (además de no modificar la pila, tal y como se explicó anteriormente en este documento). El ejemplo siguiente tiene un comportamiento similar para los autores de llamada que el ejemplo anterior. La función inicia la excepción InvalidCastException de vuelta al autor de la llamada cuando e.Data es null.
        //
        
        // catch (InvalidCastException e) when (e.Data != null)   
        // {  
            // Take some action.  
        // }

        Desde dentro de un bloque try, solo deben inicializarse las variables que se declaran en el mismo. 
        De lo contrario, puede ocurrir una excepción antes de que se complete la ejecución del bloque. 
        Por ejemplo, en el siguiente ejemplo de código, la variable n se inicializa dentro del bloque try. 
        Un intento de utilizar esta variable fuera del bloque try en la instrucción Write(n) generará un error del compilador.

            static void Main()   
            {  
                int n;  
                try   
                {  
                    // Do not initialize this variable here.  
                    n = 123;  
                }  
                catch  
                {  
                }  
                // Error: Use of unassigned local variable 'n'.  
                Console.Write(n);  
            }  


        EXCEPCIONES EN METODOS ASINCRONICOS
         
        Un método asincrónico está marcado por un modificador async y normalmente contiene una o más instrucciones o expresiones await. 
        Una expresión await aplica el operador await a Task o Task<TResult>.

        Cuando el control alcanza un await en el método asincrónico, el progreso del método se suspende hasta que la tarea esperada se completa. 
        Cuando se completa la tarea, la ejecución puede reanudarse en el método. Para más información, vea Programación asincrónica con Async y Await y 
        Controlar el flujo en los programas asincrónicos.

        La tarea completada a la que se aplica await puede encontrarse en un estado de error debido a una excepción no controlada en el método que 
        devuelve la tarea. La espera de la tarea produce una excepción. Una tarea también puede terminar en un estado cancelado si se cancela el 
        proceso asincrónico que devuelve. La espera de una tarea cancelada devuelve una OperationCanceledException. Para obtener más información 
        sobre cómo cancelar un proceso asincrónico, vea Ajustar una aplicación asincrónica (C# y Visual Basic).

        Una tarea puede encontrarse en un estado de error debido a que ocurrieron varias excepciones en el método asincrónico esperado. 
        Por ejemplo, la tarea podría ser el resultado de una llamada a Task.WhenAll. Cuando espera una tarea de este tipo, solo se captura una de las 
        excepciones y no puede predecir qué excepción se capturará.

        Ejemplo
        En el ejemplo siguiente, el bloque try contiene una llamada al método ProcessString que puede causar una excepción. 
        La cláusula catch contiene el controlador de excepciones que muestra un mensaje en la pantalla. 
        Cuando la instrucción throw se llama desde dentro MyMethod, el sistema busca la instrucción catch y muestra el mensaje Exception caught.

            class TryFinallyTest
            {
                static void ProcessString(string s)
                {
                    if (s == null)
                    {
                        throw new ArgumentNullException();
                    }
                }
    
                static void Main()
                {
                    string s = null; // For demonstration purposes.

                    try
                    {            
                        ProcessString(s);
                    }

                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Exception caught.", e);
                    }
                }
            }
            Output:
            System.ArgumentNullException: Value cannot be null.
                at TryFinallyTest.Main() Exception caught.


        En el ejemplo siguiente, se utilizan dos bloques de detección y la excepción más específica, que es la que aparece primero, es la que se captura.

        Para capturar la excepción menos específica, puede sustituir la instrucción throw en ProcessString por la siguiente instrucción: throw new Exception().

        Si coloca primero el bloque catch menos específico en el ejemplo, aparece el siguiente mensaje de error: 
        
                A previous catch clause already catches all exceptions of this or a super type ('System.Exception')


            class ThrowTest3
            {
                static void ProcessString(string s)
                {
                    if (s == null)
                    {
                        throw new ArgumentNullException();
                    }
                }

                static void Main()
                {
                    try
                    {
                        string s = null;
                        ProcessString(s);
                    }
                    // Most specific:
                    catch (ArgumentNullException e)
                    {
                        Console.WriteLine("{0} First exception caught.", e);
                    }
                    // Least specific:
                    catch (Exception e)
                    {
                        Console.WriteLine("{0} Second exception caught.", e);
                    }
                }
            }


        En el ejemplo siguiente se muestra el control de excepciones de los métodos asincrónicos. 
        Para capturar una excepción que produce una tarea asincrónica, coloque la expresión await en un bloque try y capture la excepción en un bloque catch.

        Quite la marca de comentario de la línea throw new Exception en el ejemplo para demostrar el control de excepciones. 
        La propiedad de la tarea IsFaulted se establece en True, la propiedad de la tarea Exception.InnerException se establece en la excepción, 
        y la excepción se captura en el bloque catch.

        Quite la marca de comentario de la línea throw new OperationCancelledException para ver lo que pasa cuando se cancela un proceso asincrónico. 
        La propiedad de la tarea IsCanceled se establece en true, y la excepción se captura en el bloque catch. En algunas condiciones que no son aplicables 
        a este ejemplo, la propiedad de la tarea IsFaulted se establece en true y IsCanceled se establece en false.

            public async Task DoSomethingAsync()
            {
                Task<string> theTask = DelayAsync();

                try
                {
                    string result = await theTask;
                    Debug.WriteLine("Result: " + result);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception Message: " + ex.Message);
                }
                Debug.WriteLine("Task IsCanceled: " + theTask.IsCanceled);
                Debug.WriteLine("Task IsFaulted:  " + theTask.IsFaulted);
                if (theTask.Exception != null)
                {
                    Debug.WriteLine("Task Exception Message: "
                        + theTask.Exception.Message);
                    Debug.WriteLine("Task Inner Exception Message: "
                        + theTask.Exception.InnerException.Message);
                }
            }

            private async Task<string> DelayAsync()
            {
                await Task.Delay(100);

                // Uncomment each of the following lines to
                // demonstrate exception handling.

                //throw new OperationCanceledException("canceled");
                //throw new Exception("Something happened.");
                return "Done";
            }

            // Output when no exception is thrown in the awaited method:
            //   Result: Done
            //   Task IsCanceled: False
            //   Task IsFaulted:  False

            // Output when an Exception is thrown in the awaited method:
            //   Exception Message: Something happened.
            //   Task IsCanceled: False
            //   Task IsFaulted:  True
            //   Task Exception Message: One or more errors occurred.
            //   Task Inner Exception Message: Something happened.

            // Output when a OperationCanceledException or TaskCanceledException
            // is thrown in the awaited method:
            //   Exception Message: canceled
            //   Task IsCanceled: True
            //   Task IsFaulted:  False


        En el ejemplo siguiente se muestra el control de excepciones en el que varias tareas pueden producir varias excepciones. 
        El bloque try espera la tarea devuelta por una llamada a Task.WhenAll. La tarea se completa cuando se hayan completado las 
        tres tareas a las que se aplica el método WhenAll.

        Cada una de las tres tareas produce una excepción. El bloque catch se itera a través de las excepciones, que se encuentran 
        en la propiedad Exception.InnerExceptions de la tarea devuelta por Task.WhenAll.

            public async Task DoMultipleAsync()
            {
                Task theTask1 = ExcAsync(info: "First Task");
                Task theTask2 = ExcAsync(info: "Second Task");
                Task theTask3 = ExcAsync(info: "Third Task");

                Task allTasks = Task.WhenAll(theTask1, theTask2, theTask3);

                try
                {
                    await allTasks;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception: " + ex.Message);
                    Debug.WriteLine("Task IsFaulted: " + allTasks.IsFaulted);
                    foreach (var inEx in allTasks.Exception.InnerExceptions)
                    {
                        Debug.WriteLine("Task Inner Exception: " + inEx.Message);
                    }
                }
            }

            private async Task ExcAsync(string info)
            {
                await Task.Delay(100);
    
                throw new Exception("Error-" + info);
            }

            // Output:
            //   Exception: Error-First Task
            //   Task IsFaulted: True
            //   Task Inner Exception: Error-First Task
            //   Task Inner Exception: Error-Second Task
            //   Task Inner Exception: Error-Third Task

         */

    }
}
