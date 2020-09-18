# GraphQL ASP.NET Core demo (4 - Subscriptions)

###### Subscriptions

Subscriptions provide a way to notify clients of real-time changes through a push model (usually WebSocked-based).

They are supported through the use of `IObservable<T>`. You will need a server that supports a Subscription protocol. The **GraphQL Server** project provides a .NET Core server that implements the *Apollo GraphQL* subscription protocol.


###### Adding `IObservable<T>`
Let's suppose we want to know, when animal is created. To allow such subscription, we need to create an `IObservable<T>` collection of newly created Animals.
Extract `AnimalDataStore` from general `DataStore` and add the required `IObservable<Animal>`:


```csharp
private readonly Subject<Animal> animalCreated;
public IObservable<Animal> AnimalCreated => animalCreated.AsObservable();	
```

Then we have to invoke `OnNext()` method of our field `Subject<Entities.Animal> animalCreated`, which notifies all subscribed observers about a new animal in our create method `Task<Animal> CreateAnimalAsync(Entities.Animal animal)` after its creation:

```csharp
public async Task<Entities.Animal> CreateAnimalAsync(Entities.Animal animal)
{
	var addedAnimal = await dbContext.Animals.AddAsync(animal);
	await dbContext.SaveChangesAsync();

	animalCreated.OnNext(animal);	// <-- added this line of code
	return addedAnimal.Entity;
}
```

It is also important to have `AnimalDataStore` registered in our *IoC container* with **singleton lifetime**, let's register it in `ConfigureServices()` method in `Startup.cs`:

`services.AddSingleton<IAnimalDataStore, AnimalDataStore>();`



##### GraphQL Subscriptions demo


###### Sources:

* https://graphql-dotnet.github.io/docs/getting-started/subscriptions
* https://elanderson.net/2018/08/graphql-using-net-boxed-subscriptions/
