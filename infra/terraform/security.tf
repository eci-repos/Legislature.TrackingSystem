# Legislature.TrackingSystem — GuardDuty and Security Hub for continuous security monitoring.

resource "aws_guardduty_detector" "main" {
  enable                       = true
  finding_publishing_frequency = "FIFTEEN_MINUTES"
  tags = { Name = "${local.name_prefix}-guardduty" }
}

resource "aws_securityhub_account" "main" {
  enable_default_standards     = true
  control_finding_generator    = "SECURITY_CONTROL"
  auto_enable_controls          = true
}

resource "aws_securityhub_standards_subscription" "foundational" {
  standards_arn = "arn:aws:securityhub:${var.region}::standards/aws-foundational-security-best-practices/v/1.0.0"
}

resource "aws_securityhub_standards_subscription" "cis" {
  standards_arn = "arn:aws:securityhub:${var.region}::standards/cis-aws-foundations-benchmark/v/1.4.0"
}
