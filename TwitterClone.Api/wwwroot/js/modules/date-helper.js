const monthNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

export function GetPostDate(postDate)
{
    let nowDate = Date.now();
    const dateDifference = nowDate - postDate;

    if (dateDifference < 1000*60) return "just now";
    if (dateDifference < 1000*60*60) return `${Math.floor(dateDifference/(1000*60))} minutes ago`;
    if (dateDifference < 1000*60*60*24) return `${Math.floor(dateDifference/(1000*60*60))} hours ago`;
    if (dateDifference < 1000*60*60*24*2) return `yesterday`;
    if (dateDifference < 1000*60*60*24*364) return `${monthNames[postDate.getMonth()]} ${postDate.getDate()}`;
    
    return `${monthNames[postDate.getMonth()]} ${postDate.getDate()}, ${postDate.getYear()}`
}