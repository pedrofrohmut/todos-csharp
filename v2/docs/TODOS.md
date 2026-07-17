# TODOS

- [ ] : Multiple validations errors in a single call to an usecase. Instead of the
usecase returning on the first validation error. It will accumulate the validation
errors and only after the validation is complete, it will send the list of errors.
Ex: This way you can avoid multiple tries of submitting the same form. Like you have
3 errors in a long form and you will have to try 3 times to get a passing form data
instead of having all the errors to the user of the api in the first request all at
once.
