# StatementAnalytics
This contains my statement analytics console app and wpf app. It will read in your PDF statements (So long as they are either a Barclays debit or HSBC credit account) 
and parse them into either memory (console app) or a SQLite database (WPF desktop app) where you can then quickly see all your transactions and statements without the painfully slow
banking websites or scrolling through PDFs.

The two apps are linked, the WPF app will not work without the console app, as the Console app project currently holds all of the models and helper methods for reading statements.

For now I also cannot implement a new statement type without having access to a copy of the statement, most likely I would need a few.

Please see the README files within either the [StatementAnalytics](StatementAnalytics/README.md) or 
[StatementAnalyticsDesktop.UI](StatementAnalyticsDesktop.UI/README.md) folders for more info on either the console 
or WPF app respectively.
