var sql = '';
sql += 'SELECT * FROM CUSTOMER WHERE 1=1';
if(name != '')
    sql += ` AND NAME = '${name}' `;
if(age > 0)
    sql += ` AND AGE > ${age} `;
if(v != '')
    sql += ` AND VALUE = '${v}' `;