<?php

use PHPMailer\PHPMailer\PHPMailer;
use PHPMailer\PHPMailer\SMTP;
use PHPMailer\PHPMailer\Exception;

require 'vendor/autoload.php';

$mail = new PHPMailer(true);

$emailRecipient = '';
$message = '';
$sendStatus = '';

if (isset($_POST["EmailRecipient"]) && isset($_POST["Message"])) {
    $emailRecipient = $_POST["EmailRecipient"];
    $message = $_POST["Message"];
}

try {
    $mail->SMTPDebug = SMTP::DEBUG_OFF;
    $mail->isSMTP();
    $mail->SMTPAuth = true;
    $mail->SMTPSecure = PHPMailer::ENCRYPTION_SMTPS;
    $mail->Host = "smtp.gmail.com";
    $mail->Port = 465;
    $mail->IsHTML(true);
    $mail->Username = "nikita.kuzmin2019@gmail.com";
    $mail->Password = "794613794613";

    $mail->setFrom("nikita.kuzmin2019@gmail.com");
    $mail->Subject = "Test";
    $mail->Body = $message;

    $mail->AddAddress($emailRecipient);

    $mail->send();

    echo 'Message has been sent!';
} catch (Exception $e) {
    echo "Message could not be sent. Mailer Error: {$mail->ErrorInfo}";
}