![icone](icon.png)
# Commands Panel

## NÉCÉSSITE LE [Microsoft .NET Core 3.1](https://dotnet.microsoft.com/download)

Allez sur le lien, et cliquez sur "Download .NET Core Runtime"

---

## Installation

Télécharger l'archive, extrayez les fichiers, vous avez votre command panel

## Utilisation

L'application étant minimaliste, il faut passer la souris sur la barre pour afficher le menu.

![gif](https://i.imgur.com/JReed96.gif)

Pour créer une action, il suffit de cliquer sur le bouton prévu à cet effet.

Pour utiliser une action, cliquez dessus.

![create](https://i.imgur.com/gd8PShl.gif)

Bien évidemment, le panel est déplaçable.

![drag](https://i.imgur.com/cXBR0j3.gif)

Vous avez aussi la possibilité de laisser l'application toujours au dessus, ou non...

![top](https://i.imgur.com/Ji3iJLv.gif)

## Portabilité

Pour utiliser l'application sur une clé USB en portable, créez un dossier "data" à côté de l'éxecutable avant de créer des boutons.
Note : pour une compatibilité maximum, prévoyez d'installer le runtime de Desktop netcore 3.1 sur le support USB, et lancez *CommandsPanel.dll* avec la commande `dotnet` du runtime. exemple en cmd :
```
@echo off
cd <chemin relatif vers le dossier CommandsPanel>
call <chemin relatif vers dotnet.exe> CommandsPanel.dll
```

## Plug-ins

Il est désormais possible de créer des plug-ins ! Créez un projet en .net standard 2.0 __minimum__ (netcore 3.1 préférable). Ensuite importez la dépendance du fichier *CommandsPlugin.dll* fourni avec l'installation (ne le supprimez pas, il est également nécéssaire à l'application de base). Ensuite créez un type héritant de l'interface `ICommandsPlugin`. Une fois le plugin compilé, déposez le fichier *.dll* ainsi que toutes ses références (y compris *CommandsPlugin.dll*) et ses autres fichiers nécéssaires dans le dosser "plugins" se trouvant soit dans "data" en portable, ou dans "%appdata%/CommandsPannel" autrement. Les plug-ins prennent effet uniquement après un redémarrage.
Note : les plug-ins ne devraient pas avoir à utiliser de fichiers externes, préferez les ressources intégrées au *.dll*.

## Changelog

- 0.0.4
  - can now add plugins that adds totally custom button at launch. These can't be edited inside the software as their purpose is decided in their code.

- 0.0.3
  - mode de sauvegarde changé pour anticiper les changements futurs
  - application portable possible
  - détection automatique des icones et noms des actions générées

- v0.0.2
  - correction de bugs
  - liens par défaut
  - boutons pour générer rapidement une action pour ouvrir un éxécutable

- v0.0.1
  - Application créée OwO

## Meta

- [**WildGoat07**](https://github.com/WildGoat07)

Merci à tous les [contributeurs](https://github.com/WildGoat07/CommandsPannel/contributors).

[![license](https://img.shields.io/github/license/WildGoat07/CommandsPannel?style=for-the-badge)](https://github.com/WildGoat07/CommandsPannel/blob/master/LICENSE)
