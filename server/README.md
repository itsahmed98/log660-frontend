# Laboratoire 2 – LOG660  
## Modélisation des classes de l’application en Java

---
Instructions pour exécuter le projet :

Go to the route of the project and run the following command to build the project:
```bash
dotnet build
```
Then, to run the project, use the following command:
```bash
dotnet run
```

The api will be available at `http://localhost:7273` and you can test the endpoints using tools like Postman or curl.

For swagger documentation, you can navigate to `http://localhost:7273/swagger` to see the available endpoints and their descriptions.

---

## 1. Objectif de la tâche 1

Cette première tâche a pour objectif de concevoir et d’implémenter les **classes métier** de l’application en Java, en se basant sur le **diagramme de classes UML conceptuel** réalisé lors du laboratoire 1.

L’implémentation couvre **exclusivement** les cas d’utilisation suivants du document de mise en situation :

- **Cas d’utilisation 2 : Connexion d’un utilisateur au système**
- **Cas d’utilisation 3 : Consultation interactive des films**
- **Cas d’utilisation 4 : Location de films**

L’objectif n’est pas encore d’implémenter la logique complète ni l’accès à la base de données, mais de mettre en place une structure orientée objet claire, cohérente et prête à être mappée avec un ORM lors de la tâche 2.

Un film ≠ une copie du film

Une personne ≠ un acteur ≠ un réalisateur

Une location ≠ un film

Client EST UN utilisateur

Employé EST UN utilisateur

facade.louerFilm(idClient, idFilm);


🟢 private

➡️ visible SEULEMENT dans la classe elle-même

🟡 protected

➡️ visible :

dans la classe

ET dans les classes qui héritent



---

## 2. Modélisation des classes métier

Les classes ont été conçues afin de représenter fidèlement les entités du domaine ainsi que leurs relations (héritage, associations et énumérations), conformément au diagramme UML conceptuel.

### 2.1 Hiérarchie des utilisateurs (CU2 – Connexion)

- `Utilisateur` représente toute personne pouvant se connecter au système.
- `Client` et `Employe` sont des spécialisations de `Utilisateur`.

L’héritage permet d’éviter la duplication des attributs communs (nom, courriel, mot de passe, etc.) et reflète la relation **disjointe et complète** définie dans le diagramme UML.

Relations implémentées :
- ✔️ `Client extends Utilisateur`
- ✔️ `Employe extends Utilisateur`

---

### 2.2 Films et consultation (CU3 – Consultation des films)

La classe `Film` est l’entité centrale de la consultation. Elle regroupe toutes les informations nécessaires à l’affichage et à la recherche de films.

Afin de respecter le modèle conceptuel :
- Les genres et les pays ont été modélisés comme des classes distinctes (`Genre`, `Pays`)
- Les personnes associées aux films sont représentées par une hiérarchie dédiée

Classes principales :
- `Film`
- `Genre`
- `Pays`

---

### 2.3 Personnes associées aux films

Les classes `Acteur`, `Realisateur` et `Scenariste` héritent de la classe abstraite `Personne`.

Cette modélisation permet :
- de factoriser les attributs communs (nom, date de naissance, biographie, photo)
- de représenter le fait qu’une même personne peut occuper plusieurs rôles (héritage **non disjoint et non complet**)

Relations implémentées :
- ✔️ `Acteur extends Personne`
- ✔️ `Realisateur extends Personne`
- ✔️ `Scenariste extends Personne`

---

## 3. Classes d’association

### 3.1 Role (Film ↔ Acteur)

La classe `Role` est une **classe d’association** entre `Film` et `Acteur`.

Elle existe car le nom du personnage n’a de sens que pour un acteur précis dans un film précis. Il ne s’agit donc pas d’un simple attribut, mais bien d’une entité à part entière.

Classes :
- ✔️ `Role`

---

### 3.2 Location (Client ↔ CopieFilm)

La classe `Location` est également une classe d’association. Elle représente l’acte de location effectué par un client et contient ses propres attributs :
- date de début
- date de retour
- date de retour maximale

Classes :
- ✔️ `Location`

---

## 4. Gestion des locations (CU4 – Location de films)

### 4.1 CopieFilm et Statut

Un film peut exister en plusieurs copies. La classe `CopieFilm` permet de gérer l’inventaire et l’état de chaque copie.

Le statut d’une copie est représenté à l’aide d’une énumération :
- `Statut` (`DISPONIBLE`, `PRETEE`)

L’utilisation d’une énumération garantit la validité des états possibles et améliore la robustesse du modèle.

---

### 4.2 Forfait

La classe `Forfait` définit les règles de location associées à un client :
- nombre maximal de locations
- durée maximale de location

Elle permet de centraliser les contraintes liées aux locations et de préparer l’implémentation des règles métier.

---

## 5. Pattern de conception Façade

### 5.1 Définition

Le **pattern Façade** consiste à fournir une interface unique et simplifiée pour accéder à un ensemble de classes métier complexes.

---

### 5.2 Application dans le projet

Une classe de façade (`SystemeLocationFacade`) a été implémentée afin de :
- centraliser l’accès aux fonctionnalités du système
- réduire le couplage entre la couche de présentation et la couche métier
- empêcher l’accès direct aux objets métier depuis l’interface utilisateur

Toute opération complexe doit obligatoirement transiter par la façade, notamment :
- la connexion d’un utilisateur
- la recherche et la consultation de films
- la location et le retour de films

La couche de présentation n’interagit donc jamais directement avec les classes métier.

---

## 6. Classes non directement utilisées

Certaines classes présentes dans le diagramme UML (ex. `Adresse`, `CarteCredit`, `TypeCarte`, `Employe`) ne sont pas directement sollicitées par les cas d’utilisation 2 à 4.

Elles ont néanmoins été implémentées afin de :
- préserver la cohérence globale du modèle
- respecter fidèlement le diagramme UML
- faciliter le mappage objet-relationnel lors de la tâche 2

---

## 7. Consignes respectées

### Consigne 1  
Des ajustements mineurs au diagramme UML ou au schéma relationnel sont permis afin de faciliter le mappage ORM de la tâche 2. La conception des classes tient compte de cette exigence.

### Consigne 2  
Bien que le code ne soit pas exhaustivement commenté, une attention particulière a été portée à la clarté, à la lisibilité et au choix de noms explicites pour les classes, attributs et méthodes.

---

## 8. Conclusion

Cette modélisation respecte le diagramme UML conceptuel, couvre exclusivement les cas d’utilisation 2 à 4 et met en place une architecture orientée objet claire, extensible et conforme aux principes de conception logicielle.

Elle constitue une base solide pour l’implémentation du mappage objet-relationnel et des fonctionnalités transactionnelles dans les tâches suivantes du laboratoire.
