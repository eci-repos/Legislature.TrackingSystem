# Legislature.TrackingSystem — cross-region disaster recovery.
# A cross-region RDS read replica in a DR region provides a recoverable copy of the primary
# database. The DR region topology (VPC, subnets) is declared here so the replica has a home.

variable "dr_region" {
  type    = string
  default = "us-east-1"
  description = "Disaster-recovery region for the cross-region RDS read replica."
}

provider "aws" {
  alias  = "dr"
  region = var.dr_region
}

data "aws_availability_zones" "dr" {
  provider = aws.dr
  state    = "available"
}

resource "aws_vpc" "dr" {
  provider             = aws.dr
  cidr_block           = "10.1.0.0/16"
  enable_dns_support   = true
  enable_dns_hostnames = true
  tags = { Name = "${local.name_prefix}-dr-vpc" }
}

resource "aws_subnet" "dr" {
  count             = 2
  provider          = aws.dr
  vpc_id            = aws_vpc.dr.id
  cidr_block        = cidrsubnet(aws_vpc.dr.cidr_block, 8, count.index)
  availability_zone = data.aws_availability_zones.dr.names[count.index]
  tags = { Name = "${local.name_prefix}-dr-subnet-${count.index}" }
}

resource "aws_db_subnet_group" "dr" {
  provider   = aws.dr
  name       = "${local.name_prefix}-db-dr-subnets"
  subnet_ids = aws_subnet.dr[*].id
}

resource "aws_db_instance" "dr_replica" {
  provider             = aws.dr
  identifier           = "${local.name_prefix}-db-dr"
  replicate_source_db  = aws_db_instance.main.arn
  instance_class       = "db.t3.small"
  storage_encrypted    = true
  multi_az             = false
  db_subnet_group_name = aws_db_subnet_group.dr.name
  skip_final_snapshot  = true
  tags = { Name = "${local.name_prefix}-db-dr" }
}
