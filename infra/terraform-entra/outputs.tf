output "application_client_id" {
  description = "The Entra application (client) id; set as AzureAd:ClientId."
  value       = azuread_application.lts.client_id
}

output "application_object_id" {
  description = "The Entra application object id."
  value       = azuread_application.lts.object_id
}

output "service_principal_object_id" {
  description = "The service principal object id for the app registration."
  value       = azuread_service_principal.lts.object_id
}

output "client_secret_value" {
  description = "The generated client secret value (shown once; store securely)."
  value       = azuread_application_password.lts.value
  sensitive   = true
}
