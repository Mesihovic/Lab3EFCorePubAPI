Lab 3, ASP.NET Core Web API

I den här laborationen utgick jag från lärarens uppstart med Domain, Data och migrations.

Jag började med att konfigurera databaskopplingen och aktivera Swagger. Därefter implementerade jag endpoints med Minimal API och refaktorerade sedan Author delen till en scaffoldad controller enligt uppgiftens krav. 
Author körs via scaffoldad controller.
Minimal AuthorEndpoints finns kvar från tidigare steg men används inte i slutversionen.

API:et innehåller CRUD för Authors och Books, laddar relaterad data med Include() och hanterar cykliska referenser med ReferenceHandler.IgnoreCycles. Swagger används för att testa alla endpoints. 
