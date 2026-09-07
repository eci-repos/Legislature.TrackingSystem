# Microsoft Entra ID app registration for the LTS application.
#
# This module provisions the Entra app registration, redirect URIs, and app roles that the
# application's AzureAd (Entra/OpenID Connect) boundary expects. It is declarative and intended to
# run in CI or a provisioning pipeline; it is not applied from the offline development workspace.
#
# The AzureAD provider requires network access and Azure credentials. See README.md for usage.

terraform {
  required_version = ">= 1.5"
  required_providers {
    azuread = {
      source  = "hashicorp/azuread"
      version = "~> 2.53"
    }
  }
}

provider "azuread" {
  # Credentials are supplied via the standard AzureAD provider environment variables
  # (ARM_CLIENT_ID, ARM_CLIENT_SECRET, ARM_TENANT_ID, ARM_SUBSCRIPTION_ID) or a managed identity.
}

resource "azuread_application" "lts" {
  display_name     = var.application_name
  sign_in_audience = "AzureADMyOrg"

  web {
    redirect_uris = [var.redirect_uri]
  }

  # The app roles map to LTS UserRole values. Each role value is emitted in the "roles" claim and
  # matched by AzureAd:RoleMappings in the application configuration.
  dynamic "app_role" {
    for_each = var.app_roles
    content {
      allowed_member_types = ["User", "Application"]
      description          = app_role.value.description
      display_name         = app_role.value.display_name
      value                = app_role.value.value
    }
  }

  required_resource_access {
    resource_app_id = "00000003-0000-0000-c000-000000000000" # Microsoft Graph

    dynamic "resource_access" {
      for_each = var.graph_delegated_permissions
      content {
        id   = resource_access.value
        type = "Scope"
      }
    }
  }
}

resource "azuread_application_password" "lts" {
  application_id = azuread_application.lts.id
  display_name   = "lts-client-secret"
  end_date       = timeadd(timestamp(), var.client_secret_duration)
}

resource "azuread_service_principal" "lts" {
  client_id = azuread_application.lts.client_id
}
