using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BD.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BloodCompatibility",
                columns: table => new
                {
                    compatibility_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    recipient_blood_type = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    donor_blood_type = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    component_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    is_compatible = table.Column<bool>(type: "bit", nullable: false),
                    is_Deleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BloodCom__AA12AC9993C7D67D", x => x.compatibility_id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalFacility",
                columns: table => new
                {
                    facility_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    is_Deleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MedicalF__B2E8EAAE8DF9F91D", x => x.facility_id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    is_Deleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Role__760965CC782C65DC", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "StatusBloodDonor",
                columns: table => new
                {
                    status_donor_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    is_Deleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StatusBl__DEDAE0F7C8C78C25", x => x.status_donor_id);
                });

            migrationBuilder.CreateTable(
                name: "StatusBloodInventory",
                columns: table => new
                {
                    status_inventory_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    is_Deleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StatusBl__794D33277FCFC34E", x => x.status_inventory_id);
                });

            migrationBuilder.CreateTable(
                name: "StatusBloodRequest",
                columns: table => new
                {
                    status_request_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    is_Deleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StatusBl__BB59962BCFC549EA", x => x.status_request_id);
                });

            migrationBuilder.CreateTable(
                name: "StatusNotification",
                columns: table => new
                {
                    status_notification_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    status_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    is_Deleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__StatusNo__F90F86D594BE3DF0", x => x.status_notification_id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    blood_type = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    phone = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    is_Deleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    role_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__B9BE370F4FE76943", x => x.user_id);
                    table.ForeignKey(
                        name: "FK__User__role_id__4E88ABD4",
                        column: x => x.role_id,
                        principalTable: "Role",
                        principalColumn: "role_id");
                });

            migrationBuilder.CreateTable(
                name: "BloodInventory",
                columns: table => new
                {
                    inventory_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    facility_id = table.Column<int>(type: "int", nullable: false),
                    component_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    expired_date = table.Column<DateOnly>(type: "date", nullable: false),
                    status_inventory_id = table.Column<int>(type: "int", nullable: false),
                    last_updated = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    is_Deleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    blood_type = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BloodInv__B59ACC4921691C91", x => x.inventory_id);
                    table.ForeignKey(
                        name: "FK__BloodInve__facil__797309D9",
                        column: x => x.facility_id,
                        principalTable: "MedicalFacility",
                        principalColumn: "facility_id");
                    table.ForeignKey(
                        name: "FK__BloodInve__statu__7B5B524B",
                        column: x => x.status_inventory_id,
                        principalTable: "StatusBloodInventory",
                        principalColumn: "status_inventory_id");
                });

            migrationBuilder.CreateTable(
                name: "BlogPost",
                columns: table => new
                {
                    post_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    author_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    is_Published = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    is_Deleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    is_Document = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BlogPost__3ED7876681296E8D", x => x.post_id);
                    table.ForeignKey(
                        name: "FK__BlogPost__author__00200768",
                        column: x => x.author_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "BloodRequest",
                columns: table => new
                {
                    request_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    blood_type = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    component_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    urgency_level = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    request_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    status_request_id = table.Column<int>(type: "int", nullable: false),
                    fulfilled_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    is_Deleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__BloodReq__18D3B90F70C77E71", x => x.request_id);
                    table.ForeignKey(
                        name: "FK__BloodRequ__statu__60A75C0F",
                        column: x => x.status_request_id,
                        principalTable: "StatusBloodRequest",
                        principalColumn: "status_request_id");
                    table.ForeignKey(
                        name: "FK__BloodRequ__user___5CD6CB2B",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "DonorAvailability",
                columns: table => new
                {
                    availability_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    status_donor_id = table.Column<int>(type: "int", nullable: false),
                    last_donation_date = table.Column<DateOnly>(type: "date", nullable: true),
                    recovery_reminder_date = table.Column<DateOnly>(type: "date", nullable: true),
                    available_date = table.Column<DateOnly>(type: "date", nullable: false),
                    is_Deleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__DonorAva__86E3A801CD501914", x => x.availability_id);
                    table.ForeignKey(
                        name: "FK__DonorAvai__statu__68487DD7",
                        column: x => x.status_donor_id,
                        principalTable: "StatusBloodDonor",
                        principalColumn: "status_donor_id");
                    table.ForeignKey(
                        name: "FK__DonorAvai__user___6754599E",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    notification_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    message = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    sent_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    status_notification_id = table.Column<int>(type: "int", nullable: false),
                    is_Deleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__E059842FFC2FEAB9", x => x.notification_id);
                    table.ForeignKey(
                        name: "FK__Notificat__statu__5629CD9C",
                        column: x => x.status_notification_id,
                        principalTable: "StatusNotification",
                        principalColumn: "status_notification_id");
                    table.ForeignKey(
                        name: "FK__Notificat__user___5441852A",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "DonationHistory",
                columns: table => new
                {
                    donation_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    request_id = table.Column<int>(type: "int", nullable: true),
                    facility_id = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<int>(type: "int", nullable: false),
                    donation_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    blood_type = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    component_type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    confirmed_date = table.Column<DateTime>(type: "datetime", nullable: true),
                    is_Deleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Donation__296B91DC2EA6D4FA", x => x.donation_id);
                    table.ForeignKey(
                        name: "FK__DonationH__facil__70DDC3D8",
                        column: x => x.facility_id,
                        principalTable: "MedicalFacility",
                        principalColumn: "facility_id");
                    table.ForeignKey(
                        name: "FK__DonationH__reque__6FE99F9F",
                        column: x => x.request_id,
                        principalTable: "BloodRequest",
                        principalColumn: "request_id");
                    table.ForeignKey(
                        name: "FK__DonationH__user___6EF57B66",
                        column: x => x.user_id,
                        principalTable: "User",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPost_author_id",
                table: "BlogPost",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_BloodInventory_facility_id",
                table: "BloodInventory",
                column: "facility_id");

            migrationBuilder.CreateIndex(
                name: "IX_BloodInventory_status_inventory_id",
                table: "BloodInventory",
                column: "status_inventory_id");

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequest_status_request_id",
                table: "BloodRequest",
                column: "status_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_BloodRequest_user_id",
                table: "BloodRequest",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_DonationHistory_facility_id",
                table: "DonationHistory",
                column: "facility_id");

            migrationBuilder.CreateIndex(
                name: "IX_DonationHistory_request_id",
                table: "DonationHistory",
                column: "request_id");

            migrationBuilder.CreateIndex(
                name: "IX_DonationHistory_user_id",
                table: "DonationHistory",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_DonorAvailability_status_donor_id",
                table: "DonorAvailability",
                column: "status_donor_id");

            migrationBuilder.CreateIndex(
                name: "IX_DonorAvailability_user_id",
                table: "DonorAvailability",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_status_notification_id",
                table: "Notification",
                column: "status_notification_id");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_user_id",
                table: "Notification",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_User_role_id",
                table: "User",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPost");

            migrationBuilder.DropTable(
                name: "BloodCompatibility");

            migrationBuilder.DropTable(
                name: "BloodInventory");

            migrationBuilder.DropTable(
                name: "DonationHistory");

            migrationBuilder.DropTable(
                name: "DonorAvailability");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "StatusBloodInventory");

            migrationBuilder.DropTable(
                name: "MedicalFacility");

            migrationBuilder.DropTable(
                name: "BloodRequest");

            migrationBuilder.DropTable(
                name: "StatusBloodDonor");

            migrationBuilder.DropTable(
                name: "StatusNotification");

            migrationBuilder.DropTable(
                name: "StatusBloodRequest");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
