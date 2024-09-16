# PEC3 - Un Juego de Plataformas v2


![Un Juego de Plataformas](screenshots/inicio.png "Inicio")



## Descripción

Una recreación del nivel 1-1 del clásico juego "Super Mario Bros".

Se ha intentado mantener el game feeling del movimiento de mario. Se han usado assets originales para mejorar la presentación.

## Apartados implementados

Se ha retomado la PAC2 y se ha ampliado.
Se han implementado los comportamientos faltantes opcionales (la seta y que los bloques de ladrillo fueran destruidos en este estado).

Además, se han añadido los requisitos de la PEC3, como son 2 tipos nuevos de enemigo (el enemigo nube y el erizo rojo), vidas, distintos checkpoints, se han usado partículas en las flores y bolas de fuego. 
Se ha añádido al juego la flor de fuego y animaciones adicionales
También se hace uso de tags y layers, aunque en algunos casos se hacía ya en la PEC2.

## Cómo jugar

El juego se puede controlar con teclado o mando al usar el nuevo Input System de Unity.

Los controles son los siguientes:

Teclado:

- Teclas A y D / flechas del teclado para moverse horizontalmente
- Espacio para saltar
- Shift para moverse más rápido
- E para disparar la bola de fuego (Solo con la flor)

Mando (Xbox):

- Joystick izquierdo para moverse horizontalmente
- A para saltar
- X para moverse más rápido
- B para disparar la bola de fuego (Solo con la flor)


## Capturas de pantalla y vídeo

![Un Juego de Plataformas](screenshots/Seta.png "Seta")
![Un Juego de Plataformas](screenshots/NuevosEnemigos.png "Nuevos enemigos")
![Un Juego de Plataformas](screenshots/NuevosEnemigos2.png "Nuevos enemigos")


Video: https://youtu.be/MLYnfunWaFQ

## Implementación
Las clases implementadas en la PEC2 se mantienen, aunque se han ampliado ciertas funcionalidades.

Aquí se detallan los cambios desde la PAC2:


### Clase PlayerController
Se han añadido lógicas correspondientes a los estados de Mario grande y Mario de fuego.
Se ha tenido que refactorizar cierto código para permitir las vidas y los checkpoints.
Se han ampliado en bastante el número de animaciones de Mario, ya que el cambio de hitbox lo realizamos desde el Animator. Por esta razón, se han añadido variables adicionales de animator en el código.

### Clases BrickBlock y QuestionMarkBlock
Se ha añadido la destrucción de los bricks si mario es grande.
Se han ampliado los QuestionMarkBlocks para permitir los powerUps.

### ClaseCameraFollower
Se ha adaptado la camara para los casos de respawn.

### Clase EndGameTrigger
Se ha adaptado la clase para los casos de respawn.

### Clase FallTrigger
Se ha adaptado la clase para los casos de respawn.

### Clase FireballScript
Nueva clase para controlar el comportamiento de las bolas de fuego de mario con la flor.
Se mueve hacia el lado el cual es lanzada hasta que llega o a un enemigo o a una pared. En caso de ser un enemigo, lo destruye.

### Clase MushroomScript
Nueva clase para controlar el comportamiento de las setas. 
Se comporta como un goomba en cuanto a que va moviendose hasta toparse con una pared.
Pero en este caso el sentido siempre será hacia la derecha para favorecer que Mario las pueda coger.

### Clase CloudEnemyController
Nueva clase, controlador del nuevo enemigo en forma de nube. Sigue a Mario, y de estar encima, genera enemigos de tipo erizo rojo cada cierto tiempo.

### Clase FallEnemyController
Nueva clase, controlador del nuevo enemigo "FallEnemy" con forma de erizo rojo.
Son enemigos a los que no se puede derrotar saltando, por sus puas. Solamente se pueden derrotar con bolas de fuego.
Hasta que no tocan el suelo caen de forma vertical. Una vez en el suelo se comportan como Goombas, yendo hacia un sentido aleatorio hasta toparse con una pared.

### Clase CurrentLivesUIController
Nueva clase, controlador de la interfaz de usuario para mostrar las vidas restantes de mario.
Se tienen en cuenta las vidas que tiene Mario con la variable nLives de PlayerController.


### Clases GoalScript,HUDManager,AudioManager,GoombaController
Sin cambios


## Desarrolladores

 - Francisco José Palacios Márquez

## Recursos de terceros

Sprites - https://www.spriters-resource.com/nes/supermariobros/

Sound effects - https://themushroomkingdom.net/media/smb/wav

Musica - https://downloads.khinsider.com/game-soundtracks/album/super-mario-bros

