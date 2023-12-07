# GreenThumb
 App for managing gardens with GreenThumb
 
 Slutprojekt i kursen "Databasutveckling" vid Newton Yrkeshögskola, HT-23
 
 Reflektion
I one-to-one relationen mellan "User" och "Garden", tolkade jag "Garden" som den svagare entiteten som inte kan existera utan den andra.
Det kändes mest logiskt att göra så utifrån hur flödet ser ut - det är en User som registrerar sig först och sen blir tilldelad en Garden, som då kopplas till Userns ID.
Där finns inga gardens tillgängliga utan att det finns någon User som tar hand om dem.
Om man på sikt kanske skulle vilja utöka appen så att en User kan ha flera gardens, borde det vara enklare att utöka när man har denna struktur.
Initialt valde jag att sköta tilldelningen av Garden till User genom en trigger i SQL, som bevakade när det skedde en nyregistrering i User-tabellen och då lade till en ny garden kopplat till den nya Userns användar-ID.
Men jag backade sedan på det och lade koden för det i C# istället, då jag tycker det är enklare och mer läsbart om man har allt på samma ställe.

Appen är väldigt öppen, så alla användare kan redigera växter och ta bort dem från "Huvudträdgården" (PlantWindow). 
Jag har inte implementerat några begränsningar så som att exempelvis endast en administratör kan redigera/ta bort växter, 
då jag tolkade uppgiften som att appen skulle vara tillgänglig och engagerande för alla och nu har alla användare ett gemensamt ansvar.
Nackdelen är att om en användare tar bort en växt från "Huvudträdgården" så tas den även bort från alla användares personliga trädgårdar.
Vill man begränsa detta kan man antingen lägga in ett Restricted DeleteBehavior på Plants, så att man inte kan ta bort en växt som finns i någon användares personliga trädgård, eller så introducerar man en flagga på varje användare som visar om de är Admin eller ej, och så tillåter man att endast Admins kan ta bort växter (oavsett om de finns i någons personliga trädgård).

I "Huvudträdgården" (PlantWindow) hämtas alla plantor inklusive deras instruktioner från databasen när sidan laddas in.
I sökfunktionen som implementerats på denna sida görs därför en motsvarande sökning mot databasen varje gång användaren söker.
Det känns som att det borde vara en ganska kostsam operation att göra denna sökning mot databasen varje gång man söker.
För att spara på antalet frågor mot databasen borde man kanske istället implementera en fältvariabel som sparar undan alla plantor inklusive instruktioner i en lokal lista på sidan, och sedan görs sökning mot den istället.
Nackdelen med det blir då att man får ytterligare en lista att administrera, men i en app med större dataset vore det kanske värt att göra så.

Min största utmaning med uppgiften har varit att få till en bra hantering gällande bilder för växter.
Lösningen som nu är implementerad sparar bilder i projektets bin-mapp och ska fungera för andra att öppna (hoppas jag).
Men den lösning som nu ligger innebär också att inga bilder rensas även om man byter bild på en existerande växt, så det riskerar att fylla "arkivet" i onödan.
Jag försökte länge och väl för att få till det men lyckades inte, och eftersom att det egentligen inte var en del av uppgiften, så lät jag det fungera så här.

Sammanfattningsvis så tycker jag att det har flutit på bra med arbetet. 
Denna gång försökte jag ta det lugnt och metodiskt och inte kasta mig in i problemlösning direkt, och det var en bra approach som jag ska försöka använda även i framtiden.

André Jakobsson
