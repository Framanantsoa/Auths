# LOC-AUTHS

## Description : Système d'authentification <br>

## Fonctionnalités :
* **Inscription :**
    * Créer un nouveau compte utilisateur.

* **Connexion :**
    * Authentifier un utilisateur.

* **Vérification du token :**
    * Sécuriser les routes.

* **Déconnexion :**
    * Invalider la session.

* **Prolongement de session :**
    * Éviter que l'utilisateur soit déconnecté trop souvent.

* **Récupération / modification de profil :**
    * Consulter et modifier les informations.

## Technologies :
* **Backend :** ASPNET Core v8.0
* **Base de données :** Postgresql v14

## Étapes de lancement :
* **En Local :**
    * Démarrez postgresql
    * Créez la base de données et importez le dba.dump :
        * **pg_restore -U USER -d DATABASE -v CHEMIN/dba.dump**
    * Accedez dans backend et lancez les commandes : 
        * **dotnet clean**
        * **dotnet run**
    * Utilisez l'API Rest
