# Dynamic HTML Properties Rendering Tutorial

Hello there! In this tutorial, we'll explore the usage of dynamic HTML properties to render markup in a website. We'll go through a step-by-step example using C# and ASP.NET.

## Step 1: Define Model with Defaults

Let's start by creating a model that will hold our dynamic HTML properties. In this example, we'll use C#:

```csharp
public class HTMLProps
{
    public string DataBsTheme { get; set; }
    public string HeaderClass { get; set; }
    public string NavClass { get; set; }
    public string BodyClass { get; set; }
    public string IsLoaderActive { get; set; }

    public HTMLProps()
    {
        DataBsTheme = "";
        HeaderClass = "navbar navbar-expand-lg fixed-top bg-light";
        NavClass = "";
        BodyClass = "";
        IsLoaderActive = "";
    }
}
```
## Step 2: Assign Properties in the View

Now. that we have our HTMLProps model defined, let's assign these properties in the view. We'll create an instance of HTMLProps and customize it based on our needs.

```csharp
HTMLProps props = new HTMLProps
{
    DataBsTheme = "dark",
    HeaderClass = "navbar navbar-expand-lg fixed-top",
};
```
## here is an image

![Logo](https://images.pexels.com/photos/18983851/pexels-photo-18983851/free-photo-of-a-woman-with-red-hair-sitting-on-a-chair.jpeg?auto=compress&cs=tinysrgb&w=600&lazy=load)]
