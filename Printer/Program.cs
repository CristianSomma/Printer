using System.Diagnostics;
using System.Threading;
using Printer.Models;

/*
 RISPOSTA ALLE DOMANDE:

1). Perché il tempo parallelo è minore di quello sequenziale?
    L'esecuzione parallela richiede meno tempo perché il processo viene eseguito contemporaneamente
    da più thread, a patto che il processore della macchina sia multicore.
    L'esecuzione sequenziale invece non fa uso del thread pool e limita l'esecuzione del programma ad un
    solo thread.

2). L’ordine di completamento coincide con l’ordine dei documenti?
    L'ordine di completamento dipende in caso di esecuzione parallela dal numero di pagine dei documenti, più pagine significa
    più tempo richiesto per la stampa, in esecuzione sequenziale invece dipende dall'ordine di stampa del documento e non dalla sua dimensione.

3). Cosa succede se un documento ha molte più pagine degli altri?
    La sua stampa richiederà molto più tempo, indipendentemente che venga stampato sequenzialmente o parallelamente.

4). Perché il metodo Main deve essere dichiarato async Task?
    Ogni metodo che chiama una qualche funzione asincrona deve a sua volta essere asincrono per poter aspettare il
    risultato della funzione chiamata.
 */

namespace Printer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Document[] documents = new Document[]
            {
                new Document("Final Report.pdf", 12),
                new Document("Resume.txt", 2),
                new Document("Meeting Minutes.pdf", 5),
                new Document("Client Quote.pdf", 3),
                new Document("User Manual.docx", 18),
                new Document("Project Presentation.pptx", 8),
                new Document("Supply Contract.pdf", 17)
            };


            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            await ExecutePrintingProcess(PrintSequentially, documents, ConsoleColor.Red, "SEQUENTIAL EXECUTION");
            await Task.Delay(1000);
            await ExecutePrintingProcess(PrintParallel, documents, ConsoleColor.Blue, "PARALLEL EXECUTION");
        }

        static async Task ExecutePrintingProcess(Func<IEnumerable<Document>, Task> process, IEnumerable<Document> documents, ConsoleColor textColor, string headerText)
        {
            Stopwatch sw = new Stopwatch();

            Console.ForegroundColor = textColor;
            Console.WriteLine(headerText.ToUpper());
            sw.Start();
            await process.Invoke(documents);
            sw.Stop();
            Console.WriteLine($"Time required: {sw.ElapsedMilliseconds / 1000} seconds");
        }

        static async Task<string> PrintDocument(Document document)
        {
            Console.WriteLine($"{document.Name} printing has started. [{document.Pages} pages]");

            await Task.Delay(document.Pages * 500);

            return $"The printing of {document.Name} is completed.";
        }

        static async Task PrintSequentially(IEnumerable<Document> documents)
        {
            foreach(Document document in documents)
            {
                string message = await PrintDocument(document);

                Console.WriteLine(message);
            }
        }

        static async Task PrintParallel(IEnumerable<Document> documents)
        {
            List<Task<string>> documentsInProcess = new List<Task<string>>();

            foreach(Document document in documents)
            {
                Task<string> messagePromise = PrintDocument(document);
                documentsInProcess.Add(messagePromise);
            }

            while(documentsInProcess.Count > 0)
            {
                Task<string> completedTask = await Task.WhenAny(documentsInProcess);
                Console.WriteLine(completedTask.Result);
                documentsInProcess.Remove(completedTask);
            }
        }
    }
}
