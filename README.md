# AsIKnow.Mail

Mail utility library.

## Usage ##

In order to be able to use the library functionalities it's necessary to call the _UseMailTemplates_ method during application _Configure_ providing an instance of _IServiceProvider_.

<pre>
	app.UseMailTemplates(serviceProvider);
</pre>

The library is configured by an object of type _MailOptions_ with the following attributes:

* ViewTemplateBasePath (string: "~/Views/MailTemplates"): path to the [mail template](#mail_templating) Razor views.
* SmtpServer (string): smtp server host.
* User (string): Username of the smtp server.
* Password (string): Password of the smtp server.
* NoreplayAddress (string): default _noreplay_ address to be used for emails sent without sender _From_ field set.

The library allows to send an email using an instance of the interface _IEmailSender_:

<pre>
public interface IEmailSender
{
    Task SendEmailAsync(string to, string subject, string message, string from);
    Task SendEmailAsync(string to, string subject, string message);
}
</pre>

## Mail templating<a name="mail_templating"></a> ##

In order to help Mvc application to produce email, a convenient templating sdk is provided.
It is possible to create Razor views, which will be used to render the body of sent emails.

In order to test the templates layouts the _MailTemplatesController_ is provided (available only on _Development_ environment).

By navigaing to /MailTemplates/ViewTest?viewTemplatePath&... The razor view located at _ViewTemplateBasePath_/_viewTemplatePath_ (if .cshtml is missing it will automatically be appended) will be rendered, with every query parameter (_viewTemplatePath_ excluded) addeded to the _ViewBag_ object.

In order to render a template view in a string, the convenient extensions method are provided.

Examples:

<pre>

ViewBag.val1=1;
ViewBag.val2=2;

string body = ViewBag.RenderInView("tempalte1") //renders ~/Views/MailTemplates/template1.cshtml

string body1 = MailTemplateHelper.RenderView("tempalte1", ViewBag);

string body2 = new Dictionary<string, int>(){
	{"val1", 1},
	{"val2", 2}
}.AsViewData().RenderInView("tempalte1");

bool equal = body == body1 && body == body2; //true

</pre>