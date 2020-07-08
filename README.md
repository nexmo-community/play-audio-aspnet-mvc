# Project Name

<img src="https://developer.nexmo.com/assets/images/Vonage_Nexmo.svg" height="48px" alt="Nexmo is now known as Vonage" />

<!-- Add a paragraph about the project. What does it do? Who is it for? Is it actively supported? Your reader just clicked on a random link from another web page and has no idea what Nexmo is ... -->

## Welcome to Nexmo

<!-- change "github-repo" at the end of the link to be the name of your repo, this helps us understand which projects are driving signups so we can do more stuff that developers love -->

If you're new to Nexmo, you can [sign up for a Nexmo account](https://dashboard.nexmo.com/sign-up?utm_source=DEV_REL&utm_medium=github&utm_campaign=play-audio-aspnet-mvc) and get some free credit to get you started.


<!-- add other sections as appropriate for your repo type -->
## Prerequisites

* We're going to need the latest .NET Core SDK, I'm using 3.1
* We're going to use Visual Studio Code for this tutorial, of course this will also work with Visual Studio and Visual Studio for Mac, there may just be some slightly different steps for setup and running.
* We're going to need to create a [Vonage API Account](https://dashboard.nexmo.com/sign-up). Make sure you save your `api_key` and `api_secret`
* We'll be testing this with [ngrok](https://ngrok.com/) - so go ahead and follow their instructions for setting it up.
* We're going to need [npm](https://www.npmjs.com/) to fetch the nexmo-cli

## Setup the Nexmo CLI

With npm installed we can go ahead and install and configure the nexmo cli using:

```sh
npm install nexmo-cli -g
nexmo setup API_KEY API_SECRET
```

This will get the nexmo cli setup and ready to run.

## Run Ngrok

I'm going to be throwing everything on `localhost:5000`, running ngrok will allow us to publicly access `localhost:5000`.

```sh
ngrok http --host-header=localhost:5000 5000
```

Take a note of the URL that ngrok is running on. In my case it's running on `http://7ca005ad1287.ngrok.io` so I'm going to use this as my base URL for my webhooks going forward.

## Create our Vonage Application

A Vonage Application is a construct that enables us to link route our numbers and webhooks easily. You can create an application in the [Vonage Dashboard](https://dashboard.nexmo.com/applications) or you can just make it now with the CLI.

```sh
nexmo app:create "AspNetTestApp" http://7ca005ad1287.ngrok.io/webhooks/answer http://7ca005ad1287.ngrok.io/webhooks/events
```

This is going to create a Vonage Application, it's going to then link all incoming calls to that application to the answer url, `http://7ca005ad1287.ngrok.io/webhooks/answer`, and all call events that happen on that application are going to be routed to `http://7ca005ad1287.ngrok.io/webhooks/events`. This command is going to print out two things.

1. Your application id - you can view this application id in the [Vonage Dashboard](https://dashboard.nexmo.com/applications)
2. Your application's private key. Make sure you take this and save this to a file - I'm calling mine `private.key`

### Link your Vonage Number to Your Application

When you create your account you are assigned a Vonage number. You can see this in the [numbers section of the dashboard.](https://dashboard.nexmo.com/your-numbers) Or you could alternatively just run `nexmo number:list` in your console to list your numbers. Take you Vonage Number and your Application Id and run the following:

```sh
nexmo link:app VONAGE_NUMBER APPLICATION_ID
```

With this done your calls are going to route nicely to your url.


## Configure The app

### Add Config Variables

Remember that we are using the `IConfiguration` to get our appId and our private key path. With that in mind let's open up `appsettings.json` and add the following keys to it:

```json
"APPLICATION_ID":"APPLICATION_ID",
"PRIVATE_KEY_PATH":"C:\\path\\to\\your\\private.key"
```

### Configure Kestrel or IIS Express

As I'm using VS Code, my app is naturally going to use kestrel for this, regardless if you are using kestrel or iis express let's go into `properties\launchSettings.json`and from the `PlayAudioMvc`->`applicationUrl` drop the `https://localhost:5001` endpoint - since we are not using ssl with ngrok, and we're pointing to port 5000. If you are using IIS Express, in `iisSettings`->`iisExpress` let's set the `applicationUrl` to `http://localhost:5000` and the `sslPort` to 0.

## Getting Help

We love to hear from you so if you have questions, comments or find a bug in the project, let us know! You can either:

* Open an issue on this repository
* Tweet at us! We're [@NexmoDev on Twitter](https://twitter.com/NexmoDev)
* Or [join the Nexmo Community Slack](https://developer.nexmo.com/community/slack)

## Further Reading

* Check out the Developer Documentation at <https://developer.nexmo.com>

<!-- add links to the api reference, other documentation, related blog posts, whatever someone who has read this far might find interesting :) -->


