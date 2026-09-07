output "alb_dns_name" {
  description = "DNS name of the application load balancer."
  value       = aws_lb.main.dns_name
}

output "ecs_cluster_name" {
  description = "Name of the ECS cluster."
  value       = aws_ecs_cluster.main.name
}

output "rds_endpoint" {
  description = "Endpoint of the RDS PostgreSQL instance."
  value       = aws_db_instance.main.endpoint
}

output "waf_acl_arn" {
  description = "ARN of the WAF web ACL."
  value       = aws_wafv2_web_acl.main.arn
}
