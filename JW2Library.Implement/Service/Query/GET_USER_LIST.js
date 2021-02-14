var sql = `
SELECT * 
FROM ACCOUNT
WHERE 1=1
`;

if(ID != '') 
    sql += ` AND ID = ${ID} `;

if(NAME != '')
    sql += ` AND NAME = '${NAME}' `;