-- ========================================
-- Requêtes SQL Utiles pour Identity
-- ========================================

-- 1. LISTER TOUS LES UTILISATEURS
SELECT Id, UserName, Email, City, EmailConfirmed FROM AspNetUsers ORDER BY Email;

-- 2. LISTER TOUS LES RÔLES
SELECT Id, Name FROM AspNetRoles;

-- 3. VOIR LES UTILISATEURS AVEC LEURS RÔLES
SELECT u.Email, u.City, r.Name AS RoleName
FROM AspNetUsers u
LEFT JOIN AspNetUserRoles ur ON u.Id = ur.UserId
LEFT JOIN AspNetRoles r ON ur.RoleId = r.Id;

-- 4. ASSIGNER LE RÔLE ADMIN À UN UTILISATEUR
-- Trouver UserId et RoleId puis :
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES ('USER_ID_HERE', 'ADMIN_ROLE_ID_HERE');

-- 5. PROMOUVOIR user@example.com EN ADMIN
DECLARE @UserId NVARCHAR(450) = (SELECT Id FROM AspNetUsers WHERE Email = 'user@example.com');
DECLARE @AdminRole NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE Name = 'Admin');
INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES (@UserId, @AdminRole);
