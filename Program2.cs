/*
 * Piedra, Papel y Tijera
Version Procedures ( funciones sin retorno)
 */

using System;

namespace Ejemplo
{
    class EjemploModularizacion
    {
        const string PIEDRA = "PIEDRA";
        const string PAPEL = "PAPEL";
        const string TIJERA = "TIJERA";

        static void PideJugada(in string jugador, out string jugada)
        {
            // Jugada a retornar por el jugador en OUT.


            // Flag que me indicará si el Jugador N ha realizado una jugada correcta.
            bool jugadaCorrecta;

            // Establezco el texto de las jugadas para no tener que repetirlo.
            string opciones = $"{PIEDRA}, {PAPEL}, {TIJERA}";

            // Bucle que me irá pidiendo una jugada mientras no sea correcta.
            do
            {
                // Indico el jugador que tiene que jugar y que me llega como parámetro.
                Console.WriteLine($"Jugando {jugador} ...");
                Console.Write($"\tIntroduce tú jugada ({opciones}): ");
                jugada = Console.ReadLine().ToUpper();
                jugadaCorrecta = jugada == PIEDRA || jugada == PAPEL || jugada == TIJERA;

                // Si voy a volver a pedir la entrada le indico al jugador su error.
                if (!jugadaCorrecta)
                    Console.WriteLine($"\t{jugada} no es una jugada correcta. Debe ser {opciones}");
            } while (!jugadaCorrecta);


        }


        static void GeneraJugadaMaquina(out string jugMaquina)
        {
            // Habrá muchas formas correctas de implementarlo. Pero por usar la nueva sintaxis de C#8
            // Podemos retornar el resultado de evaluar una expresión switch.
            jugMaquina = new Random().Next(0, 3) switch
            {
                0 => PIEDRA,
                1 => PAPEL,
                2 => TIJERA,
                _ => "Jugada no válida" // Este caso no se podrá dar, aquí deberíamos generar un error.
            };
        }

        static void Resultado(string jugadaUsuario, string jugadaMaquina, out string resultado)
        {

            if (jugadaMaquina == jugadaUsuario)
            {
                resultado = "Empate";
            }
            else switch (jugadaMaquina)
                {
                    case PIEDRA when jugadaUsuario == TIJERA:
                    case PAPEL when jugadaUsuario == PIEDRA:
                    case TIJERA when jugadaUsuario == PAPEL:
                        resultado = "He ganado";
                        break;
                    default:
                        resultado = "He perdido";
                        break;
                }

        }

        // El esquema algorítmico del método es análogo al de PideJugada
        static void PideNumeroJugadores(out int jugadores)
        {
            bool numeroCorrecto;
            do
            {
                Console.Write("Introduce cuantos jugadores van a participar (1 a 4): ");
                string entrada = Console.ReadLine() ?? "1";
                numeroCorrecto = int.TryParse(entrada, out jugadores);
                numeroCorrecto = numeroCorrecto && jugadores >= 1 && jugadores <= 4;
                if (!numeroCorrecto)
                    Console.WriteLine($"{entrada} no es correcto. Debe ser un valor entre 1 y 4.");
            } while (!numeroCorrecto);

        }

        static void Juega(string jugador, ref string salida)
        {
            // Juega transfiere el control a los 4 módulos en los que lo hemos subdividido
            // en el orden correcto (Izquierda a Derecha)
            // 1.- Pide Jugada a jugador N
            // 2.- Renera Jugada Máquina
            // 3.- Obtén Resultado Jugadas
            // 4.- Muestra el resultado.

            // Al modularizar el módulo queda legible, autodocumentado y ocupa menos de 10 líneas.
            PideJugada(in jugador, out string jugadaUsuario);
            GeneraJugadaMaquina(out string jugadaMaquina);
            Resultado(jugadaUsuario, jugadaMaquina, out string resultado);
            salida += $"\tYo he jugado {jugadaMaquina}\n";
            salida += $"\t{jugador} ha jugado {jugadaUsuario}\n";
            salida += $"\t{resultado}\n";
        }

        static void JuegaRonda(ref string salida)
        {
            // JuegaRonda transfiere el control a los 2 módulos en los que lo hemos subdividido...
            // 1.- Pide Numero Jugadores
            // 2.- Juega Jugador N

            int jugadores;
            PideNumeroJugadores(out jugadores);
            for (int i = 0; i < jugadores; i++)
                Juega($"Jugador_{i}", ref salida);
        }
        static void Main()
        {
            do
            {


                // Podríamos pensar que si incluimos el código de JuegaRonda aquí dentro tampoco
                // quedaría un método muy complejo.
                // Pero tendríamos un bucle dentro de un bucle y eso nos está indicando que ese
                // segundo bucle está haciendo un proceso que a su ves se puede encapsular en 
                // un módulo.

                string salida;
                salida = "INICIA LA PARTIDA ===================================================================================\n";
                JuegaRonda(ref salida);
                Console.WriteLine(salida);

                Console.WriteLine("¡¡¡ FIN PARTIDA !!!.=========================================================================\n");
                Console.WriteLine("Pulsa una tecla para jugar otra ronda. 0 para salir.");
            } while ((string)Console.ReadLine() != "0");
        }

    }
}
