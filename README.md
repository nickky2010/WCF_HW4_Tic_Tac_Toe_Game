# `WCF` Игра крестики-нолики

***

#### Разработать `SOA`-приложение, реализующее игру в крестики-нолики. 

Игроки – два пользователя, которые подключились первыми, остальные пользователи наблюдатели.

`WCF`-сервис нужно создать в виде библиотеки классов с дуплексным контрактом. Операции службы:

* подключение игрока;
* отключение игрока;
* попытка хода (возвращает `false`,  если ход невозможен);
* отправка хода;

***

#### Контракт обратного вызова должен содержать операцию  отображения хода у клиента.

Для хостинга созданной службы создать службу `Windows`.
  
Клиентское приложение реализовать как проект `Windows Forms` или `WPF`.

***
## Решение задачи:

Вид приложения после запуска:

![Alt text](/Task/Image/1.PNG?raw=true "Вид приложения после запуска")

Вид приложения во время игры:

![Alt text](/Task/Image/2.PNG?raw=true "Вид приложения во время игры")
