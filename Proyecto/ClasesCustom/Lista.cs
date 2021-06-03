using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto
{
    class Lista<T>
    {
        private T value;
        private NodoLista<T> primerNodo; // Este es el puntero a la cabeza de la lista, del tipo NodoLista (indispensable)
        private NodoLista<T> ultimoNodo; // Este es el puntero a la cola de la lista, del tipo NodoLista (recomendado)
        private string nombre;      //  atributo para darle un nombre a la estructura (este es totalmente opcional)
        public Lista(string nombreLista)  //constructor cuando se recibe unicamente el nombre de la lista
        {
            nombre = nombreLista;  //el parámetro se almacena en el atributo nombre
            primerNodo = ultimoNodo = null;  //inicializa la cabeza y la cola de la lista a NULL.
        } // fin del constructor          

        public Lista() : this("lista")  //si no se provee ningún parámetro por defecto la estructura se llama "lista"
        { //inicializa con valores por defecto, en este caso el valor por defecto será NULL.
        } // fin del constructor predeterminado  


        public void InsertarAlFrente(T insertarElemento) //método para insertar un nodo como cabeza de la lista
        {
            if (EstaVacia())  //verifica si estructura está vacía
                primerNodo = ultimoNodo = new NodoLista<T>(insertarElemento); //hace que la cabeza y la cola apunten al mismo nodo.
            else
                primerNodo = new NodoLista<T>(insertarElemento, primerNodo); //la cabeza será el nuevo nodo creado.  Este nodo señalará a quien 
                                                                          //anteriormente era la cabeza.
        } //fin del método insertar al frente

        public void InsertarAlFinal(T insertarElemento)   //método que agrega nodo como cola de la lista
        {
            if (EstaVacia())  //verifica si la lista está vacía
                primerNodo = ultimoNodo = new NodoLista<T>(insertarElemento); //hace que la cabeza y la cola apunten al mismo nodo.
            else
                ultimoNodo = ultimoNodo.Siguiente = new NodoLista<T>(insertarElemento);  //crea un nodo que almacene dato y señale a NULL
                                                                                      //el que actualmente es la cola en su puntero señalará a este nodo recién creado y luego moverá el puntero a este nuevo nodo
        } //fin del método 


        public T EliminarDelFrente() //elimina el nodo de la cabeza
        {
            if (EstaVacia()) //verifica si estructura está vacía
                throw new ExcepcionListaVacia(nombre);
            T eliminarElemento = primerNodo.Datos; //Esta línea es la almacena lo que borro
                                                        //Reestableciendo el puntero al primer elemento
            if (primerNodo == ultimoNodo) //verifica si hay un solo nodo en la lista
                primerNodo = ultimoNodo = null; //hace que la cabeza y la cola apunten a null pues no hay más nodos
            else
                primerNodo = primerNodo.Siguiente;  //el siguiente nodo será ahora la cabeza.
            return eliminarElemento; //devuelve el dato eliminado
        }


        public T EliminarDelFinal()   //elimina de la cola
        {
            if (EstaVacia())    //verifica si estructura está vacía          
                throw new ExcepcionListaVacia(nombre);

            T eliminarElemento = ultimoNodo.Datos; // obtiene los datos   
                                                        // restablece las referencias primerNodo y ultimoNodos                      
            if (primerNodo == ultimoNodo)     //verifica si hay un solo nodo en la lista         
                primerNodo = ultimoNodo = null;      //hace que la cabeza y la cola apunten a null pues no hay más nodos     
            else
            {
                NodoLista<T> actual = primerNodo;  //coloca un puntero auxiliar que ayudará a recorrer
                while (actual.Siguiente != ultimoNodo)
                { //mientras el puntero de ese auxiliar no señale al último nodo                 
                    actual = actual.Siguiente;
                } // avanza al siguiente nodo   
                ultimoNodo = actual;      // en donde se quedó el auxiliar actual se convierte en la cola de la lista
                actual.Siguiente = null;
            } // el puntero de ese nodo ahora debe señalar a null , también es el fin del else    
            return eliminarElemento; // devuelve los datos eliminados  
        } // fin del método EliminarDelFinal   


        public bool EstaVacia()       //método para establecer si estructura está vacía
        {
            return primerNodo == null;        //verifica si no la cabeza es NULL, si es así devuelve TRUE sino FALSE
        }

        public T[] ToArray()  //para poder recorrer mostrar valores de la lista
        {
            if (EstaVacia())   //verifica si lista está vacía.
            {
                //Console.WriteLine("Cola vacía");
                T[] array0 = null;
                return array0;
            }

            //Console.Write("La " + nombre + " es: ");  //envía mensaje con nombre de lista

            NodoLista<T> actual = primerNodo;   //establece un puntero auxiliar para recorrer lista.
            int contador = 0;

            while (actual != null)   //mientras ese puntero no sea NULL
            {
                contador++;
                actual = actual.Siguiente;
            }  //avanzará al siguiente nodo, también es el fin del while

            actual = primerNodo;
            T[] array1 = new T[contador];
            for (int i = 0; i < contador; i++)
            {
                array1[i] = (T)actual.Datos;
                actual = actual.Siguiente;

            }
            return array1;

        }

    } // fin de la clase Lista  


    public class ExcepcionListaVacia : ApplicationException  //excepción cuando la lista esté vacía.
    {
        public ExcepcionListaVacia(string nombre) : base("La " + nombre + " está vacía")
        {
        }
    }

}
