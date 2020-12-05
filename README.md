### ROCKET ELEVATORS CUSTOMER PORTAL ####

Quick readme! Everything related to the Portal AND the graphql API is here. 

FOr the customer portal, we used identity and scaffoled the whole registration process .Everything that is related is the the "Areas" folder.

-In Areas\Identity\Pages\Account\Register.cshtml.cs is where the validation is. We call the database, ask for the customers in JSON format and check if the returned string 
contains the email. If so, we validate :

```csharp


            var client = new HttpClient();
            var response = await client.GetAsync("https://rocket-elevators-rest-api.azurewebsites.net/api/customers");
            var content = response.Content.ReadAsStringAsync().Result;

            if (content.Contains(Input.Email)){

                if (ModelState.IsValid)
                {   .....
```

-The main (and only) controller for the pages is HomeController. For the Building (building Details) and Products route, we make graphql api calls to retreive the data, which 
which are then passed as ViewData to the views. For example :

```csharp
   Client user = await _userManager.GetUserAsync(User);
            string userId = user?.Id;

            var detailQuery = "query { customerQuery(id:" + userId + "){buildings{id admContactPhone admContactMail admContactName address{id address1}}}}";

           var response = await _helper.apiCall(detailQuery);


             if(response.IsSuccessStatusCode){
                
                var result = await response.Content.ReadAsStringAsync();
                JObject detailJSON =  JObject.Parse(result);

                ViewData["details"] = detailJSON["customerQuery"]["buildings"];
           
                return View();
             }

            SetFlash(FlashMessageType.Danger, "There was an error loading your page.");
            return LocalRedirect("/Home/Index");
```

-We first make the query, and if get a positive result, we got to the view. Otherwise, we're redirected to the root, including a flash message. To implement the flash
messages, I made a controller for that :

```csharp

    public class ControllerBase : Controller
    {
        public void SetFlash(FlashMessageType type, string text)
        {
            TempData["FlashMessage.Type"] = type;
            TempData["FlashMessage.Text"] = text;
        }
    }
```

-HomeController inherit from this controller, and we can call an instance in the various routes. It is used to pass data (TempData) to the views, which are rendered using the 
FlashMessages partial view in the shared folder:
```csharp


@{
    string text = (string)TempData["FlashMessage.Text"];
    string cssClass = "alert alert-";
    if (!string.IsNullOrWhiteSpace(text))
    {
        FlashMessageType type = (FlashMessageType)TempData["FlashMessage.Type"];
        cssClass += type.ToString().ToLower();
    }
}
@if (!string.IsNullOrWhiteSpace(text))
{
    <div class="@cssClass">
        <p>
            <button
                  type="button"
                  class="close"
                  data-dismiss="alert"
                  aria-hidden="true"
                >
                  &times;
                </button>
            @text
        </p>
    </div>
}
```

-Instead of repeating graphql and restapi calls in HomeController, there's two helper in the model folder, GRAPHQLHelper and RESTHelper. For example :

```chsarp

        public RESTHelper ()
        {
            _client = new HttpClient();
 
        }

        public async Task<HttpResponseMessage> putCall (string url, object _object)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(_object);
            var content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await _client.PutAsync(url, content);

            return response;
        }
```
-WE just call those helper and they take care of the queries.

-To pass the data from the views to the controller, there's the InformationModel and InterventionModel, both not related to any database (yet). There's just used 
in the submitForm and sendInfo route, respectively used to post the intervention form, and the updated informations. Again, we redirect with a flash message. 
I used the restAPi for those (with no new routes), and as you will, two calls are made when sending infos, one to the address table and one to the buildings table.
AS I mentionned in the video, I got that part wrong, which is frustrating considering it took WAY more time than just scaffolding a page to edit the customer infos. 

-There's not much to say about the different views. The form is almost exactly as it was, except the authorId is set to the customerId on the controller when posting the form.
This caused problem since authors and employees were in a belongs_to has_one relationship in the rails project, so I had to change that and re-seed to make it work. (Author now 
now has relationship with users).

-SO just to recap, here are the files of interest:

Areas\Identity\Pages\Account\Register.cshtml.cs and it view (registration)
Controllers\HomeController.cs (routing, where most of the work happen)
Controllers\flashController.cs (flash messages)
Models\GraphQLHelper.cs
Models\RESTHelper.cs
Models\Intervention.cs
Models\Information.cs
Views\Home\.... =>various views

- For the GraphQL API, it was just a matter of adding a few more queries and types (elevators, columns and batteries). For example :

```csharp
        Field<CustomerType>(
        "customerQuery",

        arguments: new QueryArguments(
          new QueryArgument<IdGraphType> { Name = "id"}),

        resolve: context =>
        {
          var id = context.GetArgument<long>("id");
          var customers = _db
            .Customers
            .FirstOrDefault(i => i.Id == id);

          return customers;
        });
```
Which query a customer for a given ID. Added a few GraphListType to the customer type, so that we can query all of its assets. FOr example : 
```csharp
        Field<ListGraphType<BatteryType>>(
        "batteries",

        arguments: 
            new QueryArguments(
            new QueryArgument<IntGraphType> { Name = "id" }),

        resolve: context => 
        {
            
            var batteries = _db.Batteries
                            .Where(_=>_.Building.CustomerId == context.Source.Id)
                            .ToListAsync();

            return batteries;
      });
```
Was added as a type, so that we can query the batteries directly from the customerQuery (customer have no foreign key relationship with batteries in the database.
(Btw, this is a cool thing about graphQL, that you can play with the relations between the "main" objects).


-That's about it ! Thanks you very much!


-Kem Tardif
-Kem Tardif
-And Kem Tardif. The other Kem was on vacation.

