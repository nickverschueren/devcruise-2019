using System;
using System.Collections.Generic;

namespace Euricom.DevCruise.Model
{
    public partial class DevCruiseDbContext
    {
        private void Initialize()
        {
            var speakers = InitializeSpeakers();
            Speakers.AddRange(speakers);

            var sessions = InitializeSessions();
            Sessions.AddRange(sessions);

            var slots = InitializeSlots(speakers, sessions);
            Slots.AddRange(slots);

            Database.BeginTransaction();
            SaveChanges();
            Database.CommitTransaction();
        }

        private static Speaker[] InitializeSpeakers()
        {
            var speakers = new[]
            {
                new Speaker
                {
                    Email = "christof.lauriers@euri.com",
                    FirstName = "Christof",
                    LastName = "Lauriers",
                },
                new Speaker
                {
                    Email = "david.vanheeswijck@euri.com",
                    FirstName = "David",
                    LastName = "Vanheeswijck",
                },
                new Speaker
                {
                    Email = "gaetan.vanderhaegen@euri.com",
                    FirstName = "Gaetan",
                    LastName = "Vanderhaegen",
                },
                new Speaker
                {
                    Email = "gerben.schepers@euri.com",
                    FirstName = "Gerben",
                    LastName = "Schepers",
                },
                new Speaker
                {
                    Email = "guy.vandennieuwenhof@euri.com",
                    FirstName = "Guy",
                    LastName = "Van den Nieuwenhof",
                },
                new Speaker
                {
                    Email = "hannes.holst@euri.com",
                    FirstName = "Hannes",
                    LastName = "Holst",
                },
                new Speaker
                {
                    Email = "hans.sprangers@euri.com",
                    FirstName = "Hans",
                    LastName = "Sprangers",
                },
                new Speaker
                {
                    Email = "jari.tiels@euri.com",
                    FirstName = "Jari",
                    LastName = "Tiels",
                },
                new Speaker
                {
                    Email = "jordy.rymenants@euri.com",
                    FirstName = "Jordy",
                    LastName = "Rymenants",
                },
                new Speaker
                {
                    Email = "kris.schepers@euri.com",
                    FirstName = "Kris",
                    LastName = "Schepers",
                },
                new Speaker
                {
                    Email = "luk.vanderstraeten@euri.com",
                    FirstName = "Luk",
                    LastName = "Vanderstraeten",
                },
                new Speaker
                {
                    Email = "nick.verschueren@euri.com",
                    FirstName = "Nick",
                    LastName = "Verschueren"
                },
                new Speaker
                {
                    Email = "peter.notenbaert@euri.com",
                    FirstName = "Peter",
                    LastName = "Notenbaert"
                },
                new Speaker
                {
                    Email = "thomas.claessens@euri.com",
                    FirstName = "Thomas",
                    LastName = "Claessens"
                },
                new Speaker
                {
                    Email = "wart.claes@euri.com",
                    FirstName = "Wart",
                    LastName = "Claes"
                },
                new Speaker
                {
                    Email = "wim.vanhoye@euri.com",
                    FirstName = "Wim",
                    LastName = "Van Hoye"
                },
                new Speaker
                {
                    Email = "wim.meisman@euri.com",
                    FirstName = "Wim",
                    LastName = "Meisman"
                },
                new Speaker
                {
                    Email = "yanik.ceulemans@euri.com",
                    FirstName = "Yanik",
                    LastName = "Ceulemans"
                }
            };
            return speakers;
        }

        private Session[] InitializeSessions()
        {
            var sessions = new[]
            {
                new Session { Title = "De weg naar SAFe @ Acerta", Description = "The road to SAFe at Acerta: Scaled Agile Framework® (SAFe®) empowers complex organizations to achieve the benefits of " +
                                                                                 "Lean-Agile software and systems development at scale.\nIn this session I will tell the story how SAFe has impacted our " +
                                                                                 "team and the way we work.", Code = "DC19-01" },
                new Session { Title = "Docker voor Dummies", Description = "Docker en Kubernetes zijn niet meer weg te denken uit het moderne IT-landschap. In deze workshop leer je hands-on hoe je " +
                                                                           "containers maakt, ze debugt en hoe je ze deployt naar Kubernetes. Zowel NodeJS als .NET Core containers komen aan bod.",
                                                                           Code = "DC19-02" },
                new Session { Title = "TypeScript voor Dummies", Description = "Tijdens deze workshop bekijken we wat je nodig hebt om te programmeren in TypeScript. Na een korte theoretische inleiding  " +
                                                                               "maken we onze ontwikkelomgeving klaar en gaan we aan de slag met enkele praktijkoefeningen waarin duidelijk wordt wat de " +
                                                                               "verschillen en gelijkenissen zijn met JavaScript en C#. Je mag verder ook een aantal tips & tricks verwachten om valkuilen " +
                                                                               "te vermijden.", Code = "DC19-03" },
                new Session { Title = "API Design", Description = "APIs are a key feature of modern distributed apps. They are transforming the ways how companies are built and operated, and how employees " +
                                                                  "are getting work done.\nA well designed API makes adoption of your API much easier.\nThis talk gives an overview of industry API standards, " +
                                                                  "guidelines and best practices.", Code = "DC19-04" },
                new Session { Title = "Architectuur documentatie", Description = "How to communicate your architecture to your stakeholders in an under- standable way that demonstrates that you have met their " +
                                                                                 "concerns.\nAs your appliciation goes through different stages (dev to pro, small to large), focus shifts onto different views.\n" +
                                                                                 "This talk gives a practical, practitioner - oriented guide that explains how you can handle this change with the help of " +
                                                                                 "pragmatic documentation.", Code = "DC19-05" },
                new Session { Title = "Machine Learning ", Description = "Deel 1: Een meer praktische toepassing: detecteer een uitschieter in data.\nVoorbeeld: maandelijks worden de lonen uitbetaald, de " +
                                                                         "secretaresse maakt een typo, het systeem detecteert dit en verwittigd de gebruiker.\nDeel 2: Hobbyproject: AI leert een spel spelen " +
                                                                         "zonder op voorhand de spelregels te kennen. Aan de hand van een zelfgeschreven evolving neural network (NEAT).", Code = "DC19-06" },
                new Session { Title = "Van Monoliet naar Microservices met Azure components", Description = "Hoe we bij C4T van een Monoliet Asp.Net MVC toepassing naar Microservices evolueren door middel van " +
                                                                                                            "verschillende Azure Componenten.\nAzure ServiceBus, Azure Functions, EventGrid, Blobstorage, KeyVault " +
                                                                                                            "zullen allemaal aan bod komen.", Code = "DC19-07" },
                new Session { Title = "FrankenBatch", Description = "Creating a legacy datalake by ingesting SQL Server CDC data into Google BigQuery using data contracts", Code = "DC19-08" },
                new Session { Title = "PowerBI @ Mediahuis", Description = "Hans licht een tipje van de sluier over het gebruik van PowerBI in een corporate environment: Mediahuis.\n• Hoe tabulaire modellen " +
                                                                           "gebruiken om een “single version of the truth” te bekomen?\n• Wat is het verschil tussen Self-Service BI en Self-Service Reporting?",
                                                                           Code = "DC19-09" },
                new Session { Title = "Ivy: De nieuwe Angular renderer", Description = "Het is de bedoeling om een technische sessie te geven die dieper ingaat op de werking van de nieuwe Angular renderer en " +
                                                                                       "hoe deze verschilt ten opzichte van de oude.\nWe bespreken enkele leuke features die Ivy met zich mee brengt en niet " +
                                                                                       "mogelijk waren met de vorige renderer. Verder kijken we ook naar de huidige staat van Ivy en hoe we deze zelf in onze " +
                                                                                       "projecten kunnen gebruiken.", Code = "DC19-10" },
                new Session { Title = "Alexa@Office", Description = "Je hoort het vandaag de dag wel al wat vaker: de \"slimme speaker\" is in opmars. Maar hoe werkt zo'n ding nu precies? En hoe ontwikkel je " +
                                                                    "ervoor? In deze sessie zoomen we in op het Amazon Alexa platform en bekijken we hoe je nieuwe functionaliteit kan toevoegen aan Alexa door " +
                                                                    "middel van een Azure Function App.", Code = "DC19-11" },
                new Session { Title = "Event storming workshop", Description = "Event Storming is een manier om processen te gaan modeleren. De term werd voor het eerst bedacht door Alberto Brandolini, maar " +
                                                                               "werd al snel geadopteerd door software ontwikkelaars over gans de wereld. In één of meerdere Event Storming Workshops gaan alle " +
                                                                               "betrokken partijen, zowel technisch (developers) als niet technisch (klant, project manager, ...), samen gaan uitwerken hoe een " +
                                                                               "project of proces in elkaar zit. ", Code = "DC19-12" },
                new Session { Title = "Voorbereiding hackathon ", Description = "Vorig jaar is voor de eerste keer het idee voor een  Euricom hackathon gelanceerd. Uit de enquête voor deze DevCruise bleek toch " +
                                                                                "wel een duidelijke interesse, maar om puur praktische redenen konden we dit nog niet organiseren.\nNiet getreurd, we willen er " +
                                                                                "helemaal voor gaan! Daarom deze speciale brainstormsessie om ergens volgend jaar onze eerste eigen hackathon in te richten!",
                                                                                Code = "DC19-13" },
                new Session { Title = "Hoe communiceer ik met de klant ", Description = "Een roadtrip van +-15 jaar ICT sales consultant ervaring. Zelf was ik nooit IT consultant in de field maar graag deel " +
                                                                                        "ik mijn ervaringen en anekdotes van mijn opvolgingen bij de klanten en van de consultants.", Code = "DC19-14" },
                new Session { Title = ".NET Core voor Dummies", Description = ".NET is intussen bijna 20 jaar oud, tijd om eens volledig opnieuw te beginnen! In deze workshop leer je om van niets je eerste " +
                                                                              "cross-platform .NET ASP Core 3.0 REST API met database te bouwen.\nHeb je nog nooit met .NET/C# gewerkt, of werk je al met .NET " +
                                                                              "Framework en wil je .NET Core leren kennen? Iedereen is welkom.", Code = "DC19-15" },
                new Session { Title = "Inleiding tot “the Responsibility Process”", Description = "‘Responsibility is owning your ability and power to create, choose, and attract – Christopher Avery’\nDe " +
                                                                                                  "responsibility process is een how-to model om je te helpen om persoonlijk, vrijwillig en graag " +
                                                                                                  "verantwoordelijkheid op te nemen, maar ook om dit aan te leren en om te inspireren.\nHet is een " +
                                                                                                  "wetenschappelijk onderbouwde methode, gebaseerd op 20+ jaar onderzoek, die voor iedereen van toepassing is.\n" +
                                                                                                  "Aan de hand van wat theorie, maar ook hands-on oefeningen, worden we bewust van wat onze interne mentale " +
                                                                                                  "toestand is, of kan zijn, als we confronteert worden met een probleem.\nAls resultaat sterven we ernaar om " +
                                                                                                  "vaker dingen te kunnen zeggen zoals ‘ik voel me krachtig (I feel powerfull)’ of ‘Ik voel me de eigenaar (I " +
                                                                                                  "feel ownership)’", Code = "DC19-16" },
                new Session { Title = "Bouwen van een boardgame met React", Description = "De boog hoeft niet altijd gespannen te staan. Daarom startte ik dit jaar met mijn hobbyproject: een boardgame in " +
                                                                                          "React. In deze talk laat ik zien welke nieuwe technologie ik geleerd heb, en welke lessen ik heb geleerd.",
                                                                                          Code = "DC19-17" },
                new Session { Title = "De toekomst van CSS", Description = "CSS gaat niet vooruit? Think again. Wat gaan we in de, nabije, toekomst kunnen gebruiken in onze aller geliefde frontend taal? " +
                                                                           "Spanning en sensatie in onze reis naar de toekomst!", Code = "DC19-18" },
                new Session { Title = "De Golden Circle van Simon Sinek", Description = "Wil je met meer impact communiceren, dan is deze sessie iets voor jou!\nDe Golden Circle is een denkmodel dat door" +
                                                                                        "Simon Sinek is ontwikkeld. Dit model is opgezet naar aanleiding van zijn onderzoeken naar het succes van " +
                                                                                        "de meest invloedrijke leiders en bedrijven ter wereld. Sinek ontdekte dat achter succesvolle merken allemaal " +
                                                                                        "dezelfde manier van denken, handelen en communiceren ten grondslag ligt, die compleet tegenovergesteld is " +
                                                                                        "van hoe de meerderheid denkt, handelt en communiceert. Het interessante aan dit model is, dat je dit bijna kunt " +
                                                                                        "toepassen in elke communictie.", Code = "DC19-19" },
                new Session { Title = "Estimations workshop", Description = "Wie heeft er geen problemen met het inschatten van taken? Geen nood, Wim weet daar wel raad mee! In deze workshop leer je technieken " +
                                                                            "om beter te leren schatten en pas je ze ook in praktijk toe.", Code = "DC19-20" },
                new Session { Title = "The Clean Coder groepsdiscussie", Description = "Software wordt belangrijker en belangrijker in onze samenleving, zoals de problemen met de Boeing 737 MAX nog maar eens " +
                                                                                       "hebben aangetoond. Een professionele aanpak van software ontwikkeling een absolute noodzaak geworden.\nIn \"The Clean Coder\" " +
                                                                                       "gaat Robert C. \"Uncle Bob\" Martin in op wat het juist inhoudt om jezelf professioneel software ontwikkelaar kunnen te " +
                                                                                       "noemen.\nTijdens deze workshop bespreken we in groep nog eens enkele hoofdstukken die we tijdens onze avondsessies al eens " +
                                                                                       "behandeld hebben. Als je nog niet de kans hebt gehad om 's avonds deel te nemen of als ervaringen met je collega's wil " +
                                                                                       "uitwisselen, dan is deze workshop zeker iets voor jou!", Code = "DC19-21" },
                new Session { Title = "Keynote", Description = "", Code = "DC19-22" },
                new Session { Title = "IoT: Greenhouse insights continued", Description = "Een verderzetting van de sessie van 2018. Krijg inzicht in je serre en tuin gebruik makende van Arduino en sensoren die " +
                                                                                          "beheerd worden door een IoT Edge device. Aan de hand van deze gegevens stuur je verschillende actoren aan die de omgeving " +
                                                                                          "veranderen (water, licht). In deze sessie zal ik een kleine recap geven van vorige sessie gevolgd door mijn ervaringen om " +
                                                                                          "Arduino's te connecteren met de Azure IoT omgeving. Evenals de integratie van commit tot release on the edge gebruik " +
                                                                                          "makende van Azure DevOps.", Code = "DC19-23" },
                new Session { Title = "React: What the hook", Description = "React 16.8 introduceerde hooks als een nieuwe manier om state en andere React features te gebruiken. Wat zijn hooks precies en waarom " +
                                                                            "zou je ze willen gebruiken? In deze sessie ga ik een antwoord bieden op deze vragen.", Code = "DC19-24" },
                new Session { Title = "Workshop klantgerichtheid", Description = "", Code = "DC19-25" },
                new Session { Title = "Closing keynote", Description = "", Code = "DC19-26" },
            };
            return sessions;
        }

        private static Slot[] InitializeSlots(Speaker[] speakers, Session[] sessions)
        {
            var slots = new[]
            {
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 28, 7, 30, 0),
                    EndTime = new DateTime(2019, 9, 28, 8, 0, 0),
                    Room = Room.Room1,
                    Session = sessions[21],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[15] } }
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 28, 8, 10, 0),
                    EndTime = new DateTime(2019, 9, 28, 10, 30, 0),
                    Room = Room.Room1,
                    Session = sessions[14],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[11] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 28, 8, 10, 0),
                    EndTime = new DateTime(2019, 9, 28, 10, 30, 0),
                    Room = Room.Room2,
                    Session = sessions[2],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[2] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 28, 8, 10, 0),
                    EndTime = new DateTime(2019, 9, 28, 10, 30, 0),
                    Room = Room.Room3,
                    Session = sessions[20],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[15] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 28, 14, 45, 0),
                    EndTime = new DateTime(2019, 9, 28, 15, 35, 0),
                    Room = Room.Room1,
                    Session = sessions[4],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[3] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 28, 14, 45, 0),
                    EndTime = new DateTime(2019, 9, 28, 15, 35, 0),
                    Room = Room.Room2,
                    Session = sessions[8],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[6] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 28, 14, 45, 0),
                    EndTime = new DateTime(2019, 9, 28, 15, 35, 0),
                    Room = Room.Room3,
                    Session = sessions[15],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[12] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 28, 15, 50, 0),
                    EndTime = new DateTime(2019, 9, 28, 16, 40, 0),
                    Room = Room.Room1,
                    Session = sessions[6],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[4] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 28, 15, 50, 0),
                    EndTime = new DateTime(2019, 9, 28, 16, 40, 0),
                    Room = Room.Room2,
                    Session = sessions[0],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[0] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 28, 15, 50, 0),
                    EndTime = new DateTime(2019, 9, 28, 17, 45, 0),
                    Room = Room.Room3,
                    Session = sessions[12],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[10] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 28, 16, 55, 0),
                    EndTime = new DateTime(2019, 9, 28, 17, 45, 0),
                    Room = Room.Room1,
                    Session = sessions[16],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[13] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 28, 16, 55, 0),
                    EndTime = new DateTime(2019, 9, 28, 17, 45, 0),
                    Room = Room.Room2,
                    Session = sessions[22],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[16] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 30, 07, 30, 0),
                    EndTime = new DateTime(2019, 9, 30, 10, 30, 0),
                    Room = Room.Room1,
                    Session = sessions[1],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[1] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 30, 07, 30, 0),
                    EndTime = new DateTime(2019, 9, 30, 8, 20, 0),
                    Room = Room.Room2,
                    Session = sessions[13],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[10] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 30, 07, 30, 0),
                    EndTime = new DateTime(2019, 9, 30, 09, 25, 0),
                    Room = Room.Room3,
                    Session = sessions[11],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[9] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 30, 08, 35, 0),
                    EndTime = new DateTime(2019, 9, 30, 09, 25, 0),
                    Room = Room.Room2,
                    Session = sessions[18],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[15] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 30, 09, 40, 0),
                    EndTime = new DateTime(2019, 9, 30, 10, 30, 0),
                    Room = Room.Room2,
                    Session = sessions[10],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[8] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 30, 09, 40, 0),
                    EndTime = new DateTime(2019, 9, 30, 10, 30, 0),
                    Room = Room.Room3
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 30, 14, 45, 0),
                    EndTime = new DateTime(2019, 9, 30, 15, 35, 0),
                    Room = Room.Room1,
                    Session = sessions[7],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[5] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 30, 14, 45, 0),
                    EndTime = new DateTime(2019, 9, 30, 15, 35, 0),
                    Room = Room.Room2,
                    Session = sessions[23],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[17] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 30, 14, 45, 0),
                    EndTime = new DateTime(2019, 9, 30, 16, 40, 0),
                    Room = Room.Room3,
                    Session = sessions[19],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[15] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 30, 15, 50, 0),
                    EndTime = new DateTime(2019, 9, 30, 16, 40, 0),
                    Room = Room.Room1,
                    Session = sessions[3],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[3] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 30, 15, 50, 0),
                    EndTime = new DateTime(2019, 9, 30, 16, 40, 0),
                    Room = Room.Room2,
                    Session = sessions[9],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[7] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 30, 16, 55, 0),
                    EndTime = new DateTime(2019, 9, 30, 17, 45, 0),
                    Room = Room.Room1,
                    Session = sessions[5],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[4] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 30, 16, 55, 0),
                    EndTime = new DateTime(2019, 9, 30, 17, 45, 0),
                    Room = Room.Room2,
                    Session = sessions[17],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[14] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 9, 30, 16, 55, 0),
                    EndTime = new DateTime(2019, 9, 30, 17, 45, 0),
                    Room = Room.Room3
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 10, 1, 08, 30, 0),
                    EndTime = new DateTime(2019, 10, 1, 09, 20, 0),
                    Room = Room.Room1,
                    Session = sessions[24],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[15] }}
                },
                new Slot
                {
                    StartTime = new DateTime(2019, 10, 1, 09, 30, 0),
                    EndTime = new DateTime(2019, 10, 1, 10, 0, 0),
                    Room = Room.Room1,
                    Session = sessions[25],
                    SlotSpeakers = new List<SlotSpeaker> { new SlotSpeaker { Speaker = speakers[15] }}
                }
            };
            return slots;
        }
    }
}