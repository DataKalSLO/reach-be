
# Vision
The main goal of these artifacts is to provide clarity and guidelines in building our database system. Our main goal is to provide a reliable and consistent data model for our Hourglass folks. To achieve these we want the following things:

| Feature |  Description |
|--|--|
| Reliability |  High integrity and trust in the data we provide. |
| Scalability | Planning for future growth of this application. |
| Satisfies Applications Requirements | Making sure our design covers current features and is flexible to planned or new features. |
| Efficiency | Making sure we not redundant in our information storage and our algorithms are optimize for our user usage. |

In order to achieve these features above I propose the following:

1. Minimum Level of Normalization: 3NF
2. Expected Level of Normalization: BCNF

Links:
Normal Forms - https://en.wikipedia.org/wiki/Database_normalization
3NF - https://en.wikipedia.org/wiki/Third_normal_form
5NF - https://en.wikipedia.org/wiki/Fifth_normal_form

# Database Design Overview
From the meetings we have had, I decided on 4 main entities for our application:

1. User
2. Graph
3. GeoMap
4. Story

Note: A "Meta Table" is necessary for the building of Maps and Graphs and is included in the ER Diagram. The sql statements corresponding to the diagram are in the following locations (and the order to run them in):

1. /HourglassUser/HourglassUser.sql
2. /Meta/Meta.sql
3. /Graph/Graph.sql
4. /GeoMap/GeoMap.sql
5. /Story/Story.sql

Each folder is meant to contain the DDL and DDM statements for each entity in the database.