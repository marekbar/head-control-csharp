head-control-csharp
===================

Head control - library to interpret your head move into four directions and position

Projekt ma za zadanie podjęcie stworzenia komponentu, który będzie umożliwiał wykrycie głowy człowieka
i interpretację ruchu głową na wybrane sygnały. Komponent powinien dostarczać sygnały w postaci zdarzeń, 
które mogą być konsumowane przez aplikację docelową w określonym przez nią celu, np. Zbierania informacji 
na temat, która część interfejsu aplikacji przyciąga największą uwagę, a która mniejszę. 
W przykładowej aplikacji pokazano próbę przełożenia ruchu głową na sterowanie kursorem myszki 
w obrębie okna aplikacji. Z punktu widzenia robotyki, wykonany komponent można będzie wykorzystać 
do nauczenia robota, jak ma reagować na ruch głowy osoby, którą "zobaczy oczyma kamery". 
Pomysł na taki komponent powstał po obejrzeniu video: 
https://www.youtube.com/watch?v=8110OVg9xk0&list=UU__SbuzQM3wsxMjDevEQqug, 
na którym jest pokazany robot sterowany przez program z laptopa.
