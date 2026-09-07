variable "ssl_certificate_arn" {
  type        = string
  description = "ARN of the ACM certificate for the ALB HTTPS listener."
}

variable "web_image" {
  type        = string
  description = "Container image URI for the web service (e.g. ECR repository:tag)."
}
