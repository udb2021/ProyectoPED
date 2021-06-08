using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto
{
    class Cola<T>
    {
        private T value;

        //public T MiMetodo()
        //{
        //    value = default(T);
        //    return value;
        //}
        public nodo inicio; //inicio de la cola, el nodo a atender
        public nodo final; //el último elemento insertado
        

        public Cola()
        {
            inicio = final = null;  //por defecto la estructura se crea vacía
        }

        public void Agregar(object valor)
        {
            nodo aux = new nodo(); //se crea un nodo llamado auxiliar, se resetea cada vez que se invoca el método
            aux.info = valor; //el valor recibido se almacena en el atributo info

            if (inicio == null) //verifica si la cola está vacía
            {
                inicio = aux;//el nodo creado pasa a ser el inicio
                final = aux; //el nodo creado al mismo tiempo pasa a ser el final
                aux.siguiente = null; //el puntero señalará a null pues no hay otro nodo
            }
            else
            {
                final.siguiente = aux; //al haber otros nodos, el último señalará al nuevo nodo
                aux.siguiente = null; //el puntero del nuevo señalará a null
                final = aux; //el nuevo nodo se converte en el nuevo último elemento o final
            }
        }

        public object Extraer() //método para extraer, solo puede hacerse por el frente
        {
            object valor = null; //variable para almacenar valor retirado
            if (inicio == null) //si inicio es null la cola está vacía
            {
                Console.Write("Cola vacía");
            }
            else //si cola no está vacía
            {
                valor = inicio.info; //dato almacenado se pasa a variable
                inicio = inicio.siguiente; //a quien señalaba inicio se convierte en el nuevo inicio
                //se está desplazando el puntero inicio y con eso se extrae nodo
            }
            return valor; //se muestra qué dato tenía el nodo retirado
        }

        public T[] ToArray() 
        {
                        
            if (inicio == null)
            {
                Console.WriteLine("Cola vacía");
                T[] array0 = null;
                return array0;
            }
            else
            {
                nodo puntero;
                puntero = inicio;
                int contador = 0;

                do
                {
                    contador++;
                    puntero = puntero.siguiente;
                }
                while (puntero != null);
                puntero = inicio;
                T[] array1 = new T[contador];
                for (int i = 0; i < contador; i++)
                {
                    array1[i] = (T)puntero.info;
                    puntero = puntero.siguiente;

                }
                return array1;
            }
        }
    }
}


