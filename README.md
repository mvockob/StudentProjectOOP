# GameSystem - рефакторинг ігрової підсистеми

![build](https://github.com/mvockob/StudentProjectOOP/actions/workflows/build.yml/badge.svg)
![dotnet](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![csharp](https://img.shields.io/badge/C%23-12-239120?logo=csharp&logoColor=white)
![license](https://img.shields.io/badge/license-MIT-green)
![last-commit](https://img.shields.io/github/last-commit/mvockob/StudentProjectOOP)

Навчальний проєкт: ігрова бойова система на C#, розділена на модулі,
класи - за єдиною відповідальністю (SRP), стан захищено модифікаторами доступу.

## Зміст

- [Можливості](#можливості)
- [Технології](#технології)
- [Структура](#структура)
- [Швидкий старт](#швидкий-старт)
- [Сумісність з версіями .NET](#сумісність-з-версіями-net)
- [Діаграма класів](#діаграма-класів)
- [Ролі класів](#ролі-класів)
- [Автор](#автор)
- [Ліцензія](#ліцензія)

## Можливості

* Покроковий бій двох персонажів: атака з урахуванням броні, захисна стійка (+5 броні), лікування з лімітом, унікальні здібності з множником шкоди.
* Спорядження з бонусами атаки/броні через інвентар.
* Увесь ігровий вивід і сценарій бою - через менеджер `Game`; доменні класи не знають про консоль.

## Технології

| Технологія | Версія / примітка |
|---|---|
| C# | 12 |
| .NET (таргет) | 8.0 (`net8.0`, `RollForward LatestMajor`) |
| .NET SDK для збірки | 8 або новіший |
| PlantUML | діаграма класів (`docs/`) |
| CI | GitHub Actions (Ubuntu + Windows) |

## Структура

```
Assignment.sln / Assignment.slnx
src/GameSystem/      - бібліотека: Game, Character, Equipment, Inventory,
                       CombatResolver, IAbility/Ability, IGameLogger/ConsoleGameLogger
src/Assignment.App/  - консольний застосунок (точка входу через Game)
docs/                - діаграма класів (.puml + .png) і звіт (.docx)
```

## Швидкий старт

Потрібен [.NET 8 SDK](https://dotnet.microsoft.com/download) або новіший.

```bash
dotnet build Assignment.sln
dotnet run --project src/Assignment.App
```

Очікуваний вивід:

```
========== GAME SYSTEM ==========

Merlin equipped Staff of Fire (+10 ATK, +0 ARM).
Arthur equipped Steel Shield (+0 ATK, +10 ARM).

Arthur braces for impact, increasing armor temporarily.
Merlin attacks Arthur for 15 damage!
Arthur attacks Merlin for 13 damage!
Merlin heals for 15 HP. Current HP: 70/70
Merlin uses special ability: [Fireball] on Arthur!
```

## Сумісність з версіями .NET

* Збірка: .NET 8 SDK або новіший (класичний `Assignment.sln` читають усі версії).
* Запуск: рантайм .NET 8, 9 або 10 - перевірено на 8.0.31, 9.0.20 і 10.0.12.
  Проєкт таргетує `net8.0`, а `RollForward LatestMajor` в `Assignment.App`
  дозволяє запуск на новіших рантаймах, коли 8.0 не встановлено.
* Рантайми старіші за 8.0 не підійдуть; без встановленого .NET потрібен .NET 8 SDK.

## Діаграма класів

![Діаграма класів GameSystem](docs/GameSystem.ClassDiagram.png)

Повний звіт з ролями класів: `docs/GameSystem_Report.docx`.

## Ролі класів

| Клас | Роль |
|---|---|
| `Game` | Менеджер гри: ростер, ходи, вивід, демо-сценарій |
| `Character` | Сутність персонажа: стан і дії, без `Console` |
| `Inventory` | Інвентар: предмети, суми бонусів (композиція в `Character`) |
| `Equipment` | Незмінний предмет: назва, бонуси атаки/броні |
| `IAbility` / `Ability` | Здібності як об'єкти з множником шкоди |
| `CombatResolver` | Статичні формули бою і бонус захисту |
| `IGameLogger` / `ConsoleGameLogger` | Вивід як окрема відповідальність |

## Автор

Воскобойников Марк, КН-31

## Ліцензія

MIT - див. [LICENSE](LICENSE).
