variable "application_name" {
  description = "Display name of the Entra app registration."
  type        = string
  default     = "Legislature.TrackingSystem"
}

variable "redirect_uri" {
  description = "Web redirect URI for the interactive sign-in callback (must match the client's AzureAd:RedirectUri)."
  type        = string
}

variable "client_secret_duration" {
  description = "Client secret lifetime as a Go duration string (e.g. 8760h for 12 months)."
  type        = string
  default     = "8760h"
}

variable "app_roles" {
  description = "App roles to create, each mapping to an LTS UserRole value."
  type = map(object({
    display_name = string
    description  = string
    value        = string
  }))
  default = {
    analyst = {
      display_name = "LTS Analyst"
      description  = "Prepares work products (LTS UserRole.Analyst)."
      value        = "LTS.Analyst"
    }
    reviewer = {
      display_name = "LTS Reviewer"
      description  = "Reviews work products (LTS UserRole.Reviewer)."
      value        = "LTS.Reviewer"
    }
    approver = {
      display_name = "LTS Approver"
      description  = "Approves work products (LTS UserRole.Approver)."
      value        = "LTS.Approver"
    }
    executive_reviewer = {
      display_name = "LTS Executive Reviewer"
      description  = "Performs executive review (LTS UserRole.ExecutiveReviewer)."
      value        = "LTS.ExecutiveReviewer"
    }
    financial_user = {
      display_name = "LTS Financial User"
      description  = "Prepares fiscal work products (LTS UserRole.FinancialUser)."
      value        = "LTS.FinancialUser"
    }
    read_only = {
      display_name = "LTS Read Only"
      description  = "Read-only access (LTS UserRole.ReadOnly)."
      value        = "LTS.ReadOnly"
    }
    security_administrator = {
      display_name = "LTS Security Administrator"
      description  = "Administers the solution (LTS UserRole.SecurityAdministrator)."
      value        = "LTS.SecurityAdministrator"
    }
  }
}

variable "graph_delegated_permissions" {
  description = "Microsoft Graph delegated permission ids to request (openid, profile, email, User.Read)."
  type        = set(string)
  default = [
    "37f7f235-527c-4136-accd-4a02d197296e", # openid
    "14dad69e-099b-42c9-8106-d82941f8a346", # profile
    "64a6cdd6-aab1-4aaf-94b8-3cc8405e90d0", # email
    "e1fe6dd8-ba31-4d61-89e7-88639da4683d", # User.Read
  ]
}
