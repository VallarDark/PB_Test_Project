# PB_Test_Project

The project stack is Web Api + Blazor frontend service (DDD, Onion architecture, or at least my understanding of them 😅 ).

The main idea of this project - is the ability for users to switch repository types (EF or Dapper) in real time in their account panel

Also, I added a custom Authentification and authorization system (JWT) with additional checks of users' claims. Here I add a lot of customization and experimental things so if you know a better way to do something, you welcome to comments

The Dapper repository realization wasn't added because it contains expressions and it takes a lot of time to create a custom expression visitor which could translate this expression to query code (SQL)

I continue my work on this project to be sure that it works in the best way, and architecture can be easily updated and expanded, so I will read all your comments or propositions and I'll try to answer in the closest time. All your comments are important to me.
