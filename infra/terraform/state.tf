# Legislature.TrackingSystem — Terraform state locking.
# The S3 state bucket and this DynamoDB lock table are bootstrapped out-of-band (see
# docs/deployment-runbook.md) because the backend block cannot create its own backend. This resource
# manages the lock table so it is part of the declared topology and Terraform adopts the
# bootstrapped table on the first apply.

resource "aws_dynamodb_table" "terraform_lock" {
  name         = "lts-terraform-lock"
  billing_mode = "PAY_PER_REQUEST"
  hash_key     = "LockID"

  attribute {
    name = "LockID"
    type = "S"
  }

  tags = { Name = "${local.name_prefix}-terraform-lock" }
}
